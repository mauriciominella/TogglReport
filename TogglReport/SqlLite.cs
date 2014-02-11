//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SQLite;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TogglReport.Domain.Repository
//{
//    public class SqlLite
//    {
//       private SQLiteConnection sqlite;

//       public static void InitialiseDB()
//       {
//           System.IO.File.Delete(@"C:\Repos\TogglReport\TogglReport\App_Data\ToogleDatabaseSqlLite.db");
//           System.IO.File.Copy(@"C:\Users\Mauricio\AppData\Roaming\TideSDK\com.toggl.toggldesktop\app_com.toggl.toggldesktop_0.localstorage", @"C:\Repos\TogglReport\TogglReport\App_Data\ToogleDatabaseSqlLite.db");
//       }

//       public SqlLite()
//        {
//              //This part killed me in the beginning.  I was specifying "DataSource"
//              //instead of "Data Source"


//            sqlite = new SQLiteConnection(@"Data Source=C:\Repos\TogglReport\TogglReport\App_Data\ToogleDatabaseSqlLite.db");          
//        }

//        public DataTable selectQuery(string query)
//        {
//              SQLiteDataAdapter ad = null;
//              DataTable dt = new DataTable();

//              try
//              {
//                    SQLiteCommand cmd;
//                    sqlite.Open();  //Initiate connection to the db
//                    cmd = sqlite.CreateCommand();
//                    cmd.CommandText = query;  //set the passed query
//                    ad = new SQLiteDataAdapter(cmd);
//                    ad.Fill(dt); //fill the datasource
//              }
//              catch(SQLiteException ex)
//              {
//                    //Add your exception code here.
//                  throw ex;
//              }
//              sqlite.Close();
//              return dt;
//  }
//    }
//}
