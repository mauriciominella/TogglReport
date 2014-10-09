using AutoMapper;
using Harvest.Api;
using Harvest.Api.Interfaces;
using Harvest.Api.Model.Internal;
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
    public class TimeEntryRepositoryHarvest : ITimeEntryRepository
    {
        #region Members

        private IRestRequestProvider _provider;
        private Harvest.Api.Repository _repository;

        #endregion

        #region Constructor

        public TimeEntryRepositoryHarvest()
        {
            UserSettingsService userSettingsService = new UserSettingsService();

            _provider = new RestRequestProvider("bravi", "mauricio.minella@bravi.com.br", "Maidenmba1060");
            _repository = new Harvest.Api.Repository(_provider);

            MapHarvestEntityToTimeEntryEntity();
        }

        #endregion

        #region Public Methods

        public ObservableCollection<TimeEntry> GetAll()
        {
            throw new NotImplementedException();
        }

        public TimeEntryCollection GetGroupingByDescAndDay()
        {
            throw new NotImplementedException();
        }

        public TimeEntryCollection GetGroupingByDescAndDayByDate(DateTime filterDate)
        {
            TimeEntryCollection collectionService = new TimeEntryCollection();

            ObservableCollection<TimeEntry> allItems = this.GetByDate(filterDate);

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

            return collectionService;
        }

        #endregion

        #region Private Methods

        private void MapHarvestEntityToTimeEntryEntity()
        {
            Mapper.CreateMap<DayEntryItem, TimeEntry>()
               .ForMember(d => d.id, m => m.MapFrom(s => s.Id.Value))
               .ForMember(d => d.duration, m => m.MapFrom(s => TimeSpan.FromHours(s.Hours.Value).TotalSeconds))
               .ForMember(d => d.start, m => m.MapFrom(s => s.SpentAt.Value))
               .ForMember(d => d.description, m => m.MapFrom(s => s.Notes));
        }

        private ObservableCollection<TimeEntry> GetByDate(DateTime filterDate)
        {
            DailyItem dailyItem = _repository.TimeTrackings.GetForDay(filterDate);

            ObservableCollection<TimeEntry> list = new ObservableCollection<TimeEntry>();

            foreach (DayEntryItem item in dailyItem.DayEntries.DayEntryArray)
            {
                TimeEntry mappedTimeEntry = Mapper.Map<DayEntryItem, TimeEntry>(item);
                list.Add(mappedTimeEntry);
            }

            return list;
        }

        #endregion
    }
}
