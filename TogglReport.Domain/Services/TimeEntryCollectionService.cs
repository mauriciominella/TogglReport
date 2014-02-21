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
    public class TimeEntryCollectionService : ObservableCollection<TimeEntry>
    {
        #region Members

        private IConfigurationService _configService;
        private double _totalDurationTime = 0;
        private double _totalHoursRounded = 0;

        #endregion

        #region Properties

        public double TotalDurationTime
        {
            get
            {
                return _totalDurationTime;
            }
        
        }

        public double TotalHoursRounded
        {
            get
            {
                return _totalHoursRounded;
            }
        }

        #endregion

        #region Constructores

        public TimeEntryCollectionService(IConfigurationService configService)
        {
            _configService = configService;
        }

        public TimeEntryCollectionService()
        {
            _configService = ConfigurationService.GetInstance();
        }

        #endregion

        #region Public Methods

        public void CalculateItems()
        {
            this._totalDurationTime = this.Sum(c => c.duration);

            foreach (var item in this)
            {
                item.percent = (item.duration * 100 / TotalDurationTime);
                item.hoursSuggested = (_configService.TotalHoursPerDay * (item.duration * 100 / TotalDurationTime) / 100);
                item.hoursSuggestedRounded = (_configService.TotalHoursPerDay * (item.duration * 100 / TotalDurationTime) / 100).RoundI(0.5);
            }

            this._totalHoursRounded = this.Sum(c => c.hoursSuggestedRounded);

            double roundingDifference = _configService.TotalHoursPerDay - this.TotalHoursRounded;

            double hoursToDistribute = 0.0;

            if (roundingDifference > 0)
                hoursToDistribute = 0.5;
            else if (roundingDifference < 0)
                hoursToDistribute = -0.5;

            DistributeHourToTimeEntries(roundingDifference, hoursToDistribute);

            this._totalHoursRounded = this.Sum(c => c.hoursSuggestedRounded);
        }

        private void DistributeHourToTimeEntries(double roundingDifference, double hoursToDistribute)
        {
            if (hoursToDistribute != 0)
            {
                int numberOfUnitsWithDifference = Math.Abs((int)(roundingDifference / 0.5));

                //Distribuite the difference to all records
                for (int i = 0; i < numberOfUnitsWithDifference; i++)
                {
                    this[i].hoursSuggestedRounded += hoursToDistribute;
                }
            }
        }

        #endregion
    }
}
