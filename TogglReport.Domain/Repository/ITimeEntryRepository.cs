using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglReport.Domain.Model;
using TogglReport.Domain.Services;

namespace TogglReport.Domain.Repository
{
    public interface ITimeEntryRepository
    {
        ObservableCollection<TimeEntry> GetAll();

        TimeEntryCollectionService GetGroupingByDescAndDay();

        TimeEntryCollectionService GetGroupingByDescAndDayByDate(DateTime date);

    }
}
