using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglReport.Domain.Services;

namespace TogglReport.Domain.ViewModel
{
    [Export(typeof(IScreen))]
    public class ConfigurationViewModel : Screen
    {
        #region Members

        private string _instalationPath = String.Empty;
        private string _notificationMessage;
        private string _apiToken = String.Empty;

        private UserSettingsService _userSettingsService;

        #endregion

        #region Overrides / ViewModel Lifecycle

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            this.ApiToken = _userSettingsService.GetApiToken();
        }

        #endregion

        #region Properties

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

        #region Constructor

        public ConfigurationViewModel()
        {
            DisplayName = "Configuration";
            _userSettingsService = new UserSettingsService();
            SetInstalationPath();
        }

        #endregion

        #region Public Methods

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

        #region Private Methods

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

    }
}
