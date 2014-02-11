using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglReport.Domain.Services
{
    public class ConfigurationService : IConfigurationService
    {
        #region Members

        private FileInfo _togglDatabasePath = null;
        private FileInfo _togglTemporaryDatabasePath = null;

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

        #endregion

        #region Public Methods

        public void Load()
        {
            this._togglDatabasePath = new FileInfo(@"C:\Users\Mauricio\AppData\Roaming\TideSDK\com.toggl.toggldesktop\app_com.toggl.toggldesktop_0.localstorage");
            //this._togglTemporaryDatabasePath = new FileInfo(@"C:\Repos\TogglReport\TogglReport\App_Data\ToogleDatabaseSqlLite.db");
            this._togglTemporaryDatabasePath = new FileInfo(Environment.CurrentDirectory + "\\ToogleDatabaseSqlLite.db");
            
        }

        #endregion

    }
}
