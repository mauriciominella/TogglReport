using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        private bool _loadingData = false;
        private string _instalationPath = String.Empty;
        private string _notificationMessage;
        private string _apiToken = String.Empty;

        private UserSettingsService _userSettingsService;

        #endregion

        #region Properties

        public string NotificationMessage
        {
            get { return _notificationMessage; }
            set
            {
                _notificationMessage = value;
                NotifyOfPropertyChange(() => NotificationMessage);
            }
        }

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

        public string InstalationPath
        {
            get
            {
                return _instalationPath;
            }
            set
            {
                _instalationPath = value;
                NotifyOfPropertyChange(() => InstalationPath);
            }
        }

        public string ApiToken
        {
            get
            {
                return _apiToken;
            }
            set
            {
                _apiToken = value;
                NotifyOfPropertyChange(() => ApiToken);
            }
        }

        #endregion

        #region Constructors

        public ShellViewModel()
        {
            _userSettingsService = new UserSettingsService();
            SetInstalationPath();
        }

        #endregion

        #region Overrides / ViewModel Lifecycle

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);

            this.CurrentQueryDate = DateTime.Now.AddDays(-1);
            this.Items = new ObservableCollection<TimeEntry>();
            this.ApiToken = _userSettingsService.GetApiToken();
            FilterItems();
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

                    Caliburn.Micro.Execute.OnUIThread(ShowNoRecordsMessage);;
                });
        }

        private void FilterItemsAsync(System.Action sucesscallBack, Action<Exception> errorCallback)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    ITimeEntryRepository timeEntryCollection = new TimeEntryRepositoryWeb();

                    TimeEntryCollectionService timeentries = timeEntryCollection.GetGroupingByDescAndDayByDate(CurrentQueryDate);
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
            //var box = new ConfirmationBoxViewModel();
            
            //WindowManager wm = new WindowManager();
            //var result = wm.ShowDialog(box);
            this.NotificationMessage = "There are not records";
            WindowManager wm = new WindowManager();
            
        }

        private void SetInstalationPath()
        {
            //Get the assembly information
            System.Reflection.Assembly assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();

            //Location is where the assembly is run from 
            string assemblyLocation = assemblyInfo.Location;

            //CodeBase is the location of the ClickOnce deployment files
            Uri uriCodeBase = new Uri(assemblyInfo.CodeBase);
            InstalationPath = Path.GetDirectoryName(uriCodeBase.LocalPath.ToString());
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

        public void SaveApiToken(string apiToken)
        {
            _userSettingsService.SaveApiToken(apiToken);
            this.ApiToken = apiToken;
        }

        public bool CanSaveApiToken(string apiToken)
        {
            return apiToken.Length > 0;
        }

        #endregion

    }
}
