using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglReport.Domain.Services;

namespace TogglReport.Domain.Repository
{
    public class DbHelper
    {

        #region Singleton Implementation

        private static DbHelper instance = new DbHelper();

        public static DbHelper GetInstance()
        {
            return instance;
        }

        #endregion

        private ConfigurationService _configService;

        public DbHelper()
        {
            _configService = new ConfigurationService();
            _configService.Load();
        }

       public void InitialiseDB()
       {
           System.IO.File.Delete(_configService.TogglTemporaryDatabasePath.FullName);
           System.IO.File.Copy(_configService.TogglDatabasePath.FullName, _configService.TogglTemporaryDatabasePath.FullName);
       }


        public DataTable selectQuery(string query)
        {
            SQLiteConnection sqlite = new SQLiteConnection(@"Data Source=" + _configService.TogglTemporaryDatabasePath.FullName);          

              SQLiteDataAdapter ad = null;
              DataTable dt = new DataTable();

              try
              {
                    SQLiteCommand cmd;
                    sqlite.Open();  //Initiate connection to the db
                    cmd = sqlite.CreateCommand();
                    cmd.CommandText = query;  //set the passed query
                    ad = new SQLiteDataAdapter(cmd);
                    ad.Fill(dt); //fill the datasource
              }
              catch(SQLiteException ex)
              {
                    //Add your exception code here.
                  throw ex;
              }
              sqlite.Close();
              return dt;
  }
    }
}
