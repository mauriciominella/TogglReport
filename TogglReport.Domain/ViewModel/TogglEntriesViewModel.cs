using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TogglReport.Domain.Model;
using TogglReport.Domain.Repository;
using TogglReport.Domain.Services;

namespace TogglReport.Domain.ViewModel
{
    [Export(typeof(IScreen))]
    public class TogglEntriesViewModel : Screen
    {
        #region Members

        private DateTime _currentQueryDate;
        private ObservableCollection<TimeEntry> _items;
        private string _dayOfWeek = String.Empty;
        private bool _loadingData = false;
        private TimesheetCalculationService _calculationService;

        #endregion

        #region Overrides / ViewModel Lifecycle

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);

            this.CurrentQueryDate = DateTime.Now.AddDays(-1);
            this.Items = new ObservableCollection<TimeEntry>();
            FilterItems();
        }

        #endregion

        #region Properties

        public DateTime CurrentQueryDate
        {
            get { return _currentQueryDate; }
            set
            {
                _currentQueryDate = value;
                DayOfWeek = _currentQueryDate.ToString("ddd");
                NotifyOfPropertyChange(() => CurrentQueryDate);
            }
        }

        public ObservableCollection<TimeEntry> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                NotifyOfPropertyChange(() => Items);
            }
        }

        public string DayOfWeek
        {
            get
            {
                return _dayOfWeek;
            }
            set
            {
                _dayOfWeek = value;
                NotifyOfPropertyChange(() => DayOfWeek);
            }
        }

        public bool LoadingData
        {
            get
            {
                return _loadingData;
            }
            set
            {
                _loadingData = value;
                NotifyOfPropertyChange(() => LoadingData);
            }
        }


        #endregion

        #region Constructor

        public TogglEntriesViewModel()
        {
            DisplayName = "Toggl Time";
            _calculationService = new TimesheetCalculationService();
        }

        #endregion

        #region Public Methods

        public void Today()
        {
            DateTime today = DateTime.Now;
            this.CurrentQueryDate = today;
            FilterItems();
        }

        public void All()
        {
            ITimeEntryRepository timeEntryCollection = new TimeEntryRepositoryWeb();
            this.Items.Clear();

            TimeEntryCollection timeentries = timeEntryCollection.GetGroupingByDescAndDay();

            this.Items = timeentries;
        }

        public void Yesterday()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);
            this.CurrentQueryDate = yesterday;
            FilterItems();
        }

        public void PreviousDay()
        {
            CurrentQueryDate = CurrentQueryDate.AddDays(-1);
            FilterItems();
        }

        public void NextDay()
        {
            CurrentQueryDate = CurrentQueryDate.AddDays(1);
            FilterItems();
        }

        public void CalculateTimeSheet()
        {
            _calculationService.CalculateItems(this.Items.ToList());
            this.Items = new ObservableCollection<TimeEntry>(_calculationService.CalculatedList);
        }

        public void ReloadData()
        {
            this.FilterItems();
        }

        #endregion

        #region Private Methods

        private void FilterItems()
        {
            LoadingData = true;
            this.Items.Clear();

            FilterItemsAsync(
                () =>
                {
                    LoadingData = false;
                },
                (Exception ex) =>
                {
                    LoadingData = false;

                    Caliburn.Micro.Execute.OnUIThread(ShowNoRecordsMessage); ;
                });
        }

        private void FilterItemsAsync(System.Action sucesscallBack, Action<Exception> errorCallback)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    ITimeEntryRepository timeEntryCollection = new TimeEntryRepositoryWeb();

                    TimeEntryCollection timeentries = timeEntryCollection.GetGroupingByDescAndDayByDate(CurrentQueryDate);
                    this.Items = timeentries;
                }
                catch (ThreadAbortException) { /* dont report on this */ }
                catch (Exception ex)
                {
                    if (errorCallback != null) errorCallback(ex);
                }
                // note: this will not be called if the thread is aborted
                if (sucesscallBack != null) sucesscallBack();

            });
        }

        private void ShowNoRecordsMessage()
        {

        }

        #endregion
    }
}
