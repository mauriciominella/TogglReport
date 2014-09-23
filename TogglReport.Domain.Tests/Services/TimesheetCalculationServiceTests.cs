using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TogglReport.Domain.Tests.Fakes;
using FizzWare.NBuilder;
using TogglReport.Domain.Model;
using System.Collections.Generic;

namespace TogglReport.Domain.Services.Tests
{
    [TestClass]
    public class TimesheetCalculationServiceTests
    {
        private const double expectedTotalHoursRounded = 7.5;


        [TestMethod]
        public void Rounded_Hours_Correctness_Using_Especfic_Scenario_1()
        {
            ConfigurationServiceBuilder configServiceBuilder = new ConfigurationServiceBuilder();
            IConfigurationService configService = configServiceBuilder.Build();
            TimesheetCalculationService calculationService = new TimesheetCalculationService(configService);
            List<TimeEntry> timeEntryList = new List<TimeEntry>();

            BuildDataForTestingScenario1(timeEntryList);

            calculationService.CalculateItems(timeEntryList);

            Assert.AreEqual(expectedTotalHoursRounded, calculationService.TotalHoursRounded);
        }


        [TestMethod]
        public void Rounded_Hours_Correctness_Using_Especfic_Scenario_2()
        {
            ConfigurationServiceBuilder configServiceBuilder = new ConfigurationServiceBuilder();
            IConfigurationService configService = configServiceBuilder.Build();
            TimesheetCalculationService calculationService = new TimesheetCalculationService(configService);
            List<TimeEntry> timeEntryList = new List<TimeEntry>();

            BuildDataForTestingScenario2(timeEntryList);

            calculationService.CalculateItems(timeEntryList);

            Assert.AreEqual(expectedTotalHoursRounded, calculationService.TotalHoursRounded);
        }


        [TestMethod]
        public void Rounded_Hours_Correctness_Using_A_Random_Scenario()
        {
            ConfigurationServiceBuilder configServiceBuilder = new ConfigurationServiceBuilder();
            IConfigurationService configService = configServiceBuilder.Build();
            TimesheetCalculationService calculationService = new TimesheetCalculationService(configService);
            List<TimeEntry> timeEntryList = new List<TimeEntry>();

            BuildDataForTestingARandomScenario(timeEntryList);

            calculationService.CalculateItems(timeEntryList);

            Assert.AreEqual(expectedTotalHoursRounded, calculationService.TotalHoursRounded);
        }

        private void BuildDataForTestingScenario1(List<TimeEntry> timeEntryList)
        {
            timeEntryList.Add(new Model.TimeEntry()
            {
                description = "144449:6.4.0.4 - Set system context on login into Student Self-Management and Learner Portals",
                start = new DateTime(2014, 2, 11),
                duration = 20284,
                isTimesheet = true
            });

            timeEntryList.Add(new Model.TimeEntry()
            {
                description = "Stand Up",
                start = new DateTime(2014, 2, 11),
                duration = 611,
                isTimesheet = true
            });

            timeEntryList.Add(new Model.TimeEntry()
            {
                description = "144269:6.4.4.2 Apps In Client: Limit editing of a Learner's applications where context doesn't match",
                start = new DateTime(2014, 2, 11),
                duration = 8298,
                isTimesheet = true
            });
        }

        private void BuildDataForTestingScenario2(List<TimeEntry> timeEntryList)
        {
            timeEntryList.Add(new Model.TimeEntry()
            {
                description = "231386:PublishAssessmentResults message publishing when it is not viewable by Learner",
                start = new DateTime(2014, 2, 19),
                duration = 5724,
                isTimesheet = true
            });

            timeEntryList.Add(new Model.TimeEntry()
            {
                description = "230322:E2E_NLP_Grading Scheme Code is not updated in 158 and 162 instances",
                start = new DateTime(2014, 2, 19),
                duration = 9231,
                isTimesheet = true
            });

            timeEntryList.Add(new Model.TimeEntry()
            {
                description = "Meeting about Tafe Bugs",
                start = new DateTime(2014, 2, 19),
                duration = 1981,
                isTimesheet = true
            });

            timeEntryList.Add(new Model.TimeEntry()
            {
                description = "231764: E2E_ACN_CHESSN number is not saved in ebs4",
                start = new DateTime(2014, 2, 19),
                duration = 7323,
                isTimesheet = true
            });
        }

        private void BuildDataForTestingARandomScenario(List<TimeEntry> timeEntryList)
        {
            var randomGenereatedList = Builder<TimeEntry>.CreateListOfSize(100)
                           .TheFirst(1)
                                  .With(x => x.description = "Dinamically generated time entry")
                                  .And(x => x.start = DateTime.Now)
                                  .And(x => x.duration = 1320)
                                  .And(x => x.isTimesheet = true)
                            .TheNext(1)
                                    .With(x => x.description = "Dinamically generated time entry")
                                  .And(x => x.start = DateTime.Now)
                                  .And(x => x.duration = 1320)
                                  .And(x => x.isTimesheet = true)
                            .Build();


            foreach (var item in randomGenereatedList)
	        {
                timeEntryList.Add(item);
	        }

        }
    }
}
