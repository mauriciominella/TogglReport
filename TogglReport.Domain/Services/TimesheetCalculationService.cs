using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglReport.Domain.Model;
using TogglReport.Domain.Extensions;

namespace TogglReport.Domain.Services
{
    public class TimesheetCalculationService
    {
        #region Members

        private IConfigurationService _configService;
        private double _totalDurationTime = 0;
        private double _totalHoursRounded = 0;

        private List<TimeEntry> _originalTimeEntryList = new List<TimeEntry>();

        private List<TimeEntry> _outputTimeEntryList = new List<TimeEntry>();

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

        public List<TimeEntry> CalculatedList
        {
            get
            {
                return _outputTimeEntryList;
            }
        }

        #endregion
        
        #region Constructores

        public TimesheetCalculationService(IConfigurationService configService)
        {
            _configService = configService;
        }

        public TimesheetCalculationService()
        {
            _configService = ConfigurationService.GetInstance();
            _configService.Load();
        }

        #endregion

        #region Public Methods

        public void CalculateItems(List<TimeEntry> listToCalculate, DateTime selectedDate)
        {
            _originalTimeEntryList = listToCalculate;

            _outputTimeEntryList.Clear();
            _outputTimeEntryList.AddRange(_originalTimeEntryList.FindAll(t => t.isTimesheet));

            if (_outputTimeEntryList.Count > 0)
            {
                this._totalDurationTime = _outputTimeEntryList.Sum(c => c.duration);

                foreach (var item in _outputTimeEntryList)
                {
                    item.percent = (item.duration * 100 / TotalDurationTime);
                    item.hoursSuggested = (_configService.GetTotalHourForCurrentDay(selectedDate) * (item.duration * 100 / TotalDurationTime) / 100);
                    item.hoursSuggestedRounded = (_configService.GetTotalHourForCurrentDay(selectedDate) * (item.duration * 100 / TotalDurationTime) / 100).RoundI(0.25);
                }

                this._totalHoursRounded = _outputTimeEntryList.Sum(c => c.hoursSuggestedRounded);

                double roundingDifference = _configService.GetTotalHourForCurrentDay(selectedDate) - this.TotalHoursRounded;

                double hoursToDistribute = 0.0;

                if (roundingDifference > 0)
                    hoursToDistribute = 0.25;
                else if (roundingDifference < 0)
                    hoursToDistribute = -0.25;

                DistributeHourToTimeEntries(roundingDifference, hoursToDistribute);

                this._totalHoursRounded = _outputTimeEntryList.Sum(c => c.hoursSuggestedRounded);
            }
        }

        private void DistributeHourToTimeEntries(double roundingDifference, double hoursToDistribute)
        {
            if (hoursToDistribute != 0)
            {
                int numberOfUnitsWithDifference = Math.Abs((int)(roundingDifference / 0.25));

                //Distribuite the difference to all records
                for (int i = 0; i < numberOfUnitsWithDifference; i++)
                {
                    _outputTimeEntryList[i].hoursSuggestedRounded += hoursToDistribute;
                }
            }
        }

        #endregion
    }
}
