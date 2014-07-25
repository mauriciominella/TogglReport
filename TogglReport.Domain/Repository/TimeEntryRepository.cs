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
    public class TimeEntryRepository
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

        public TimeEntryCollectionService GetGroupingByDescAndDay()
        {
            ObservableCollection<TimeEntry> allItems = this.GetAll();
            TimeEntryCollectionService collectionService = new TimeEntryCollectionService();

            var query2 = from a in allItems
                         group a by new { a.description, startDate = new DateTime(a.start.Year, a.start.Month, a.start.Day) } into g
                         select new { description = g.Key.description, start = g.Key.startDate, duration = g.Sum(c => c.duration )};

            foreach (var item in query2)
            {
                collectionService.Add(new TimeEntry
                {
                    description = item.description,
                    duration = item.duration,
                    start = item.start,
                });

           }

            collectionService.CalculateItems();
            
            return collectionService;

        }

        public TimeEntryCollectionService GetGroupingByDescAndDayForToday()
        {
            TimeEntryCollectionService collectionService = new TimeEntryCollectionService();

            DateTime today = DateTime.Now;

            IEnumerable<TimeEntry> groupedByDescAndDay = this.GetGroupingByDescAndDay().Where(c => c.start.Day == today.Day && c.start.Month == today.Month && c.start.Year == today.Year);

            foreach (var item in groupedByDescAndDay)
            {
                collectionService.Add(item);
            }

            collectionService.CalculateItems();

            return collectionService;

        }


        public TimeEntryCollectionService GetGroupingByDescAndDayForYesterday()
        {
            TimeEntryCollectionService collectionService = new TimeEntryCollectionService();

            DateTime yesterday = DateTime.Now.AddDays(-1);

            IEnumerable<TimeEntry> groupedByDescAndDay = this.GetGroupingByDescAndDay().Where(c => c.start.Day == yesterday.Day && c.start.Month == yesterday.Month && c.start.Year == yesterday.Year);

            var totalDurationSum = groupedByDescAndDay.Sum(c => c.duration);

            foreach (var item in groupedByDescAndDay)
            {
                collectionService.Add(item);
            }

            collectionService.CalculateItems();

            return collectionService;

        }

        public TimeEntryCollectionService GetGroupingByDescAndDayByDate(DateTime date)
        {
            TimeEntryCollectionService collectionService = new TimeEntryCollectionService();

            IEnumerable<TimeEntry> groupedByDescAndDay = this.GetGroupingByDescAndDay().Where(c => c.start.Day == date.Day && c.start.Month == date.Month && c.start.Year == date.Year);

            var totalDurationSum = groupedByDescAndDay.Sum(c => c.duration);

            foreach (var item in groupedByDescAndDay)
            {
                collectionService.Add(item);
            }

            collectionService.CalculateItems();

            return collectionService;

        }
    }}
