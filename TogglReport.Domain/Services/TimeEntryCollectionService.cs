using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglReport.Domain.Services;
using TogglReport.Domain.Extensions;
using System.Collections.ObjectModel;
using TogglReport.Domain.Model;

namespace TogglReport.Domain.Services
{
    public class TimeEntryCollection : ObservableCollection<TimeEntry>
    {
        #region Members

        private IConfigurationService _configService;


        #endregion

        #region Constructores

        public TimeEntryCollection(IConfigurationService configService)
        {
            _configService = configService;
            this.CollectionChanged += TimeEntryCollection_CollectionChanged;
        }

        public TimeEntryCollection()
        {
            _configService = ConfigurationService.GetInstance();
            _configService.Load();
            this.CollectionChanged += TimeEntryCollection_CollectionChanged;
        }

        #endregion

        #region Private Methods

        private void TimeEntryCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in  e.NewItems.Cast<TimeEntry>())
            {
                item.isTimesheet = true;
            }
        }


        #endregion

    }
}
