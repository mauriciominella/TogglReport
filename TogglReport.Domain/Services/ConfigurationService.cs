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
            HoursPerWeekDay = new Dictionary<DayOfWeek, double>();
        }

        public ConfigurationService(FileInfo togglDatabasePath, FileInfo togglTemporaryDatabasePath, double totalHoursPerDay)
        {
            this._togglDatabasePath = togglDatabasePath;
            this._togglTemporaryDatabasePath = togglTemporaryDatabasePath;

            HoursPerWeekDay = new Dictionary<DayOfWeek, double>();
            HoursPerWeekDay.Add(DayOfWeek.Sunday, 0);
            HoursPerWeekDay.Add(DayOfWeek.Monday, 7.5);
            HoursPerWeekDay.Add(DayOfWeek.Tuesday, 8.75);
            HoursPerWeekDay.Add(DayOfWeek.Wednesday, 8);
            HoursPerWeekDay.Add(DayOfWeek.Thursday, 8);
            HoursPerWeekDay.Add(DayOfWeek.Friday, 5.25);
            HoursPerWeekDay.Add(DayOfWeek.Saturday, 0);
        }

        #endregion

        #region Members

        private FileInfo _togglDatabasePath = null;
        private FileInfo _togglTemporaryDatabasePath = null;
        private double _totalHoursPerDay;

        public Dictionary<DayOfWeek, double> HoursPerWeekDay { get; set; }

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

        //private double TotalHoursPerDay
        //{
        //    get
        //    {
        //        return _totalHoursPerDay;
        //    }
        //}

        public double GetTotalHourForCurrentDay(DateTime date)
        {
            return this.HoursPerWeekDay[date.DayOfWeek];
        }

        #endregion

        #region Public Methods

        public void Load()
        {
            HoursPerWeekDay = new Dictionary<DayOfWeek, double>();

            if (String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["TogglDatabasePath"]))
                throw new System.Configuration.SettingsPropertyNotFoundException("TogglDatabasePath");

            //if (String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["TogglApiToken"]))
            //    throw new System.Configuration.SettingsPropertyNotFoundException("TogglApiToken");
             
            //this._togglDatabasePath = new FileInfo(@"C:\Users\Mauricio\AppData\Roaming\TideSDK\com.toggl.toggldesktop\app_com.toggl.toggldesktop_0.localstorage");

            this._togglDatabasePath = new FileInfo(System.Configuration.ConfigurationManager.AppSettings["TogglDatabasePath"]);
            this._togglTemporaryDatabasePath = new FileInfo(Environment.CurrentDirectory + "\\ToogleDatabaseSqlLite.db");
            //this._totalHoursPerDay = 5.25;

        }

        #endregion

    }
}
