using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglReport.Domain.Model;
using TogglReport.Domain.Repository;
using TogglReport.Domain.Services;

namespace TogglReport.Domain.ViewModel
{
    public class ShellViewModel : Screen
    {
        #region Members

        private DateTime _currentQueryDate;
        private ObservableCollection<TimeEntry> _items;
        private string _dayOfWeek = String.Empty;

        #endregion

        #region Properties

        public DateTime CurrentQueryDate
        {
            get { return _currentQueryDate; }
            set { 
                _currentQueryDate = value;
                DayOfWeek = _currentQueryDate.ToString("ddd");
                NotifyOfPropertyChange(() => CurrentQueryDate);
            }
        }

        public ObservableCollection<TimeEntry> Items
        {
            get { return _items; }
            set {
                _items = value;
                NotifyOfPropertyChange(() => Items);
            }
        }

        public string DayOfWeek
        {
            get {
                return _dayOfWeek;
            }
            set
            {
                _dayOfWeek = value;
                NotifyOfPropertyChange(() => DayOfWeek);
            }
        }

        #endregion

        #region Constructors

        public ShellViewModel()
        {

        }

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

        #region Private Methods

        private void FilterItems()
        {
            TimeEntryRepository timeEntryCollection = new TimeEntryRepository();
            this.Items.Clear();

            try
            {
                TimeEntryCollectionService timeentries = timeEntryCollection.GetGroupingByDescAndDayByDate(CurrentQueryDate);
                this.Items = timeentries;
            }
            catch (Exception)
            {
                ShowNoRecordsMessage();
            }

        }

        private void ShowNoRecordsMessage()
        {
            var box = new ConfirmationBoxViewModel();
            WindowManager wm = new WindowManager();
            var result = wm.ShowDialog(box);
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
            TimeEntryRepository timeEntryCollection = new TimeEntryRepository();
            this.Items.Clear();

            TimeEntryCollectionService timeentries = timeEntryCollection.GetGroupingByDescAndDay();

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

        #endregion

    }
}
