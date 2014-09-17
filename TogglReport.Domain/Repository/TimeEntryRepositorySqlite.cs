using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglReport.Domain.Model;
using TogglReport.Domain.Extensions;
using TogglReport.Domain.Services;

namespace TogglReport.Domain.Repository
{
    public class TimeEntryRepositorySqlite : ITimeEntryRepository
    {
        private const double totalHoursDay = 7.5;

        public ObservableCollection<TimeEntry> GetAll()
        {
            ObservableCollection<TimeEntry> timeEntryCollection = new ObservableCollection<TimeEntry>();

            DbHelper db = DbHelper.GetInstance();
            DataTable dt = db.selectQuery("SELECT * FROM ItemTable WHERE Key like 'TimeEntries%'");

            if (dt != null)
            {
                foreach (DataRow item in dt.Rows)
                {
                    try
                    {
                        string jsonStringValue = item["value"].ToString();
                        TimeEntry timeEntry = Newtonsoft.Json.JsonConvert.DeserializeObject<TimeEntry>(jsonStringValue);
                        timeEntryCollection.Add(timeEntry);
                    }
                    catch (Exception)
                    {
                    }

                }
            }

            return timeEntryCollection;
        }

        public TimeEntryCollection GetGroupingByDescAndDay()
        {
            ObservableCollection<TimeEntry> allItems = this.GetAll();
            TimeEntryCollection timeEntryCollection = new TimeEntryCollection();

            var query2 = from a in allItems
                         group a by new { a.description, startDate = new DateTime(a.start.Year, a.start.Month, a.start.Day) } into g
                         select new { description = g.Key.description, start = g.Key.startDate, duration = g.Sum(c => c.duration) };

            foreach (var item in query2)
            {
                timeEntryCollection.Add(new TimeEntry
                {
                    description = item.description,
                    duration = item.duration,
                    start = item.start,
                });

            }

            return timeEntryCollection;

        }

        public TimeEntryCollection GetGroupingByDescAndDayByDate(DateTime date)
        {
            TimeEntryCollection timeEntryCollection = new TimeEntryCollection();

            IEnumerable<TimeEntry> groupedByDescAndDay = this.GetGroupingByDescAndDay().Where(c => c.start.Day == date.Day && c.start.Month == date.Month && c.start.Year == date.Year);

            var totalDurationSum = groupedByDescAndDay.Sum(c => c.duration);

            foreach (var item in groupedByDescAndDay)
            {
                timeEntryCollection.Add(item);
            }

            return timeEntryCollection;

        }
    }
}
