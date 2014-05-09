using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglReport.Domain.Services
{
    public class ConfigurationService : IConfigurationService
    {
        #region Singleton Implementation

        private static IConfigurationService instance = new ConfigurationService();

        public static IConfigurationService GetInstance()
        {
            return instance;
        }

        #endregion

        #region Constructors

        public ConfigurationService()
        {

        }

        public ConfigurationService(FileInfo togglDatabasePath, FileInfo togglTemporaryDatabasePath, double totalHoursPerDay)
        {
            this._togglDatabasePath = togglDatabasePath;
            this._togglTemporaryDatabasePath = togglTemporaryDatabasePath;
            this._totalHoursPerDay = totalHoursPerDay;
        }

        #endregion

        #region Members

        private FileInfo _togglDatabasePath = null;
        private FileInfo _togglTemporaryDatabasePath = null;
        private double _totalHoursPerDay;

        #endregion

        #region Properties

        public FileInfo TogglDatabasePath
        {
            get
            {
                return _togglDatabasePath;
            }
        }


        public FileInfo TogglTemporaryDatabasePath
        {
            get
            {
                return _togglTemporaryDatabasePath;
            }
        }

        public double TotalHoursPerDay
        {
            get
            {
                return _totalHoursPerDay;
            }
        }

        #endregion

        #region Public Methods

        public void Load()
        {
            if (String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["TogglDatabasePath"]))
                throw new System.Configuration.SettingsPropertyNotFoundException("TogglDatabasePath");
            
            //this._togglDatabasePath = new FileInfo(@"C:\Users\Mauricio\AppData\Roaming\TideSDK\com.toggl.toggldesktop\app_com.toggl.toggldesktop_0.localstorage");

            this._togglDatabasePath = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["TogglDatabasePath"]);
            this._togglTemporaryDatabasePath = new FileInfo(Environment.CurrentDirectory + "\\ToogleDatabaseSqlLite.db");
            this._totalHoursPerDay = 7.5;
        }

        #endregion


        
    }
}
