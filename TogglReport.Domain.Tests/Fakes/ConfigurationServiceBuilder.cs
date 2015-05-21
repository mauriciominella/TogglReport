using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglReport.Domain.Services;

namespace TogglReport.Domain.Tests.Fakes
{
    public class ConfigurationServiceBuilder : IConfigurationService
    {
        private FileInfo _togglDatabasePath = null;
        private FileInfo _togglTemporaryDatabasePath = null;
        private double _totalHoursPerDay = 7.5;

        #region IConfigurationService Members

        public FileInfo TogglDatabasePath
        {
            get { return _togglDatabasePath; }
        }

        public FileInfo TogglTemporaryDatabasePath
        {
            get { return _togglTemporaryDatabasePath; }
        }

        public double TotalHoursPerDay
        {
            get { return _totalHoursPerDay; }
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Builder Methods

        public ConfigurationService Build()
        {
            return new ConfigurationService(_togglDatabasePath, _togglTemporaryDatabasePath, _totalHoursPerDay);
        }

        public ConfigurationServiceBuilder WithNonExistingDatabasePath()
        {
            _togglDatabasePath = new FileInfo(@"c:\windows\temp\");
            return this;
        }

        public ConfigurationServiceBuilder WithNonExistingTemporaryDatabasePath()
        {
            _togglTemporaryDatabasePath = new FileInfo(@"c:\windows\temp\");
            return this;
        }

        public ConfigurationServiceBuilder WithSevenAndHalfTotalHoursPerDay()
        {
            _totalHoursPerDay = 7.5;
            return this;
        }

        public static implicit operator ConfigurationService(ConfigurationServiceBuilder instance)
        {
            return instance.Build();
        }


        #endregion

        public string TogglApiToken
        {
            get { throw new NotImplementedException(); }
        }


        public double GetTotalHourForCurrentDay(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
