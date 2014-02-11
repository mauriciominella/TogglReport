using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglReport.Domain.Model;
using TogglReport.Domain.Extensions;

namespace TogglReport.Domain.Repository
{
    public class TimeEntryRepository
    {
        private const double totalHoursDay = 7.5;

        public ObservableCollection<TimeEntry> GetAll()
        {
            ObservableCollection<TimeEntry> timeEntryCollection = new ObservableCollection<TimeEntry>();

            SqlLite db = new SqlLite();
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

        public ObservableCollection<TimeEntry> GetGroupingByDescAndDay()
        {
            ObservableCollection<TimeEntry> allItems = this.GetAll();
            ObservableCollection<TimeEntry> groupedItems = new ObservableCollection<TimeEntry>();

            var query2 = from a in allItems
                         group a by new { a.description, startDate = new DateTime(a.start.Year, a.start.Month, a.start.Day) } into g
                         select new { description = g.Key.description, start = g.Key.startDate, duration = g.Sum(c => c.duration )};

            var totalDurationSum = query2.Sum(c => c.duration);

            foreach (var item in query2)
            {
                groupedItems.Add(new TimeEntry
                {
                    description = item.description,
                    duration = item.duration,
                    start = item.start,
                    percent = (item.duration * 100 / totalDurationSum)
                });
            }

            return groupedItems;

        }

        public ObservableCollection<TimeEntry> GetGroupingByDescAndDayForToday()
        {
            ObservableCollection<TimeEntry> groupedItems = new ObservableCollection<TimeEntry>();

            DateTime today = DateTime.Now;

            IEnumerable<TimeEntry> groupedByDescAndDay = this.GetGroupingByDescAndDay().Where(c => c.start.Day == today.Day && c.start.Month == today.Month && c.start.Year == today.Year);

            var totalDurationSum = groupedByDescAndDay.Sum(c => c.duration);

            foreach (var item in groupedByDescAndDay)
            {
                groupedItems.Add(new TimeEntry
                {
                    description = item.description,
                    duration = item.duration,
                    start = item.start,
                    percent = (item.duration * 100 / totalDurationSum),
                    hoursSuggested = (totalHoursDay * (item.duration * 100 / totalDurationSum) / 100),
                    hoursSuggestedRounded = (totalHoursDay * (item.duration * 100 / totalDurationSum) / 100).RoundI(0.5)
                });
            }

            return groupedItems;

        }


        public ObservableCollection<TimeEntry> GetGroupingByDescAndDayForYesterday()
        {
            ObservableCollection<TimeEntry> groupedItems = new ObservableCollection<TimeEntry>();

            DateTime yesterday = DateTime.Now.AddDays(-1);

            IEnumerable<TimeEntry> groupedByDescAndDay = this.GetGroupingByDescAndDay().Where(c => c.start.Day == yesterday.Day && c.start.Month == yesterday.Month && c.start.Year == yesterday.Year);

            var totalDurationSum = groupedByDescAndDay.Sum(c => c.duration);

            foreach (var item in groupedByDescAndDay)
            {
                groupedItems.Add(new TimeEntry
                {
                    description = item.description,
                    duration = item.duration,
                    start = item.start,
                    percent = (item.duration * 100 / totalDurationSum),
                    hoursSuggested = (totalHoursDay * (item.duration * 100 / totalDurationSum) / 100),
                    hoursSuggestedRounded = (totalHoursDay * (item.duration * 100 / totalDurationSum) / 100).RoundI(0.5)
                });
            }

            return groupedItems;

        }
    }}
