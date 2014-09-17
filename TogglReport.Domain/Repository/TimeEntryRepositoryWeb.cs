using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TogglReport.Domain.Model;
using TogglReport.Domain.Services;

namespace TogglReport.Domain.Repository
{
    public class TimeEntryRepositoryWeb : ITimeEntryRepository
    {
        private const string url = "https://www.toggl.com/api/v8/time_entries";

        private string _userPass = String.Empty;
        private string _userpassB64 = String.Empty;
        private string _authHeader = String.Empty;

        public TimeEntryRepositoryWeb()
        {
            UserSettingsService userSettingsService = new UserSettingsService();

            _userPass = userSettingsService.GetApiToken() + ":api_token";
            _userpassB64 = Convert.ToBase64String(Encoding.Default.GetBytes(_userPass.Trim()));
            _authHeader = "Basic " + _userpassB64;
        }

        public ObservableCollection<TimeEntry> GetAll()
        {
            ObservableCollection<TimeEntry> list = new ObservableCollection<TimeEntry>();

            HttpWebRequest authRequest = (HttpWebRequest)WebRequest.Create(url);
            authRequest.Headers.Add("Authorization", _authHeader);
            authRequest.Method = "GET";
            authRequest.ContentType = "application/json";
            //authRequest.Credentials = CredentialCache.DefaultNetworkCredentials;

            try
            {
                var response = (HttpWebResponse)authRequest.GetResponse();
                string result = null;

                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader sr = new StreamReader(stream);
                    result = sr.ReadToEnd();
                    sr.Close();

                    list = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<TimeEntry>>(result);
            
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

            return list;
        }

        public TimeEntryCollection GetGroupingByDescAndDay()
        {
            ObservableCollection<TimeEntry> allItems = this.GetAll();
            TimeEntryCollection collectionService = new TimeEntryCollection();

            var query2 = from a in allItems
                         group a by new { a.description, startDate = new DateTime(a.start.Year, a.start.Month, a.start.Day) } into g
                         select new { description = g.Key.description, start = g.Key.startDate, duration = g.Sum(c => c.duration) };

            foreach (var item in query2)
            {
                collectionService.Add(new TimeEntry
                {
                    description = item.description,
                    duration = item.duration,
                    start = item.start,
                });

            }

            //collectionService.CalculateItems();

            return collectionService;
        }

        public TimeEntryCollection GetGroupingByDescAndDayByDate(DateTime date)
        {
            TimeEntryCollection collectionService = new TimeEntryCollection();

            IEnumerable<TimeEntry> groupedByDescAndDay = this.GetGroupingByDescAndDay().Where(c => c.start.Day == date.Day && c.start.Month == date.Month && c.start.Year == date.Year);

            var totalDurationSum = groupedByDescAndDay.Sum(c => c.duration);

            foreach (var item in groupedByDescAndDay)
            {
                collectionService.Add(item);
            }

            //collectionService.CalculateItems();

            return collectionService;
        }
    }
}
