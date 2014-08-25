using Ahgora.Api.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ahgora.Api.Repository
{
    public class TimesheetRepository
    {

        #region Members

        private string _username = String.Empty;
        private string _password = String.Empty;

        #endregion

        #region Properties

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        #endregion

        #region Constructor

        public TimesheetRepository(string username, string password)
        {
            this._username = username;
            this._password = password;
        }

        #endregion

        #region Public Methods

        public RootObject GetAll()
        {
            RootObject timesheetCollection = new RootObject();

            string url = "https://horas.bravi.com.br/api/ponto";

            String encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(this.Username + ":" + this.Password));
            string authHeader = "Basic " + encoded;

            HttpWebRequest authRequest = (HttpWebRequest)WebRequest.Create(url);
            authRequest.Headers.Add("Authorization", authHeader);
            authRequest.Method = "GET";
            authRequest.ContentType = "application/json";


            try
            {
                var response = (HttpWebResponse)authRequest.GetResponse();
                string result = null;

                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream);
                    result = sr.ReadToEnd();
                    sr.Close();

                    timesheetCollection = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(result);

                }

                if (null != result)
                {
                    System.Diagnostics.Debug.WriteLine(result.ToString());
                }
                // Get the headers
                object headers = response.Headers;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message + "\n" + e.ToString());
            }

            return timesheetCollection;
        }

        #endregion

   
    }
}
