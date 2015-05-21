using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglReport.Domain.Services
{
    public interface IConfigurationService
    {
        #region Properties

        FileInfo TogglDatabasePath { get; }
        FileInfo TogglTemporaryDatabasePath { get; }

        #endregion

        #region Public Methods

        void Load();

        double GetTotalHourForCurrentDay(DateTime date);
    
        #endregion
    }
}
