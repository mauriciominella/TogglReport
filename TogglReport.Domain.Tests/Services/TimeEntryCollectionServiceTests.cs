using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TogglReport.Domain.Tests.Fakes;
using FizzWare.NBuilder;
using TogglReport.Domain.Model;

namespace TogglReport.Domain.Services.Tests
{
    [TestClass]
    public class TimeEntryCollectionTests
    {
        private const double expectedTotalHoursRounded = 7.5;


        [TestMethod]
        public void Rounded_Hours_Correctness_Using_Especfic_Scenario()
        {
            ConfigurationServiceBuilder configServiceBuilder = new ConfigurationServiceBuilder();

            IConfigurationService configService = configServiceBuilder.Build();

            TimeEntryCollectionService timeEntryCollectionService = new TimeEntryCollectionService(configService);

            BuildDataForTestingScenario1(timeEntryCollectionService);

            timeEntryCollectionService.CalculateItems();

            Assert.AreEqual(expectedTotalHoursRounded, timeEntryCollectionService.TotalHoursRounded);
        }


        [TestMethod]
        public void Rounded_Hours_Correctness_Using_A_Random_Scenario()
        {
            ConfigurationServiceBuilder configServiceBuilder = new ConfigurationServiceBuilder();

            IConfigurationService configService = configServiceBuilder.Build();

            TimeEntryCollectionService timeEntryCollectionService = new TimeEntryCollectionService(configService);

            BuildDataForTestingARandomScenario(timeEntryCollectionService);

            timeEntryCollectionService.CalculateItems();

            Assert.AreEqual(expectedTotalHoursRounded, timeEntryCollectionService.TotalHoursRounded);
        }

        private void BuildDataForTestingScenario1(TimeEntryCollectionService timeEntryCollectionService)
        {
            timeEntryCollectionService.Add(new Model.TimeEntry()
            {
                description = "144449:6.4.0.4 - Set system context on login into Student Self-Management and Learner Portals",
                start = new DateTime(2014, 2, 11),
                duration = 20284,
            });

            timeEntryCollectionService.Add(new Model.TimeEntry()
            {
                description = "Stand Up",
                start = new DateTime(2014, 2, 11),
                duration = 611,
            });

            timeEntryCollectionService.Add(new Model.TimeEntry()
            {
                description = "144269:6.4.4.2 Apps In Client: Limit editing of a Learner's applications where context doesn't match",
                start = new DateTime(2014, 2, 11),
                duration = 8298,
            });
        }


        private void BuildDataForTestingARandomScenario(TimeEntryCollectionService timeEntryCollectionService)
        {
            var randomGenereatedList = Builder<TimeEntry>.CreateListOfSize(100)
                           .TheFirst(1)
                                  .With(x => x.description = "Dinamically generated time entry")
                                  .And(x => x.start = DateTime.Now)
                                  .And(x => x.duration = 1320)
                            .TheNext(1)
                                    .With(x => x.description = "Dinamically generated time entry")
                                  .And(x => x.start = DateTime.Now)
                                  .And(x => x.duration = 1320)
                            .Build();


            foreach (var item in randomGenereatedList)
	        {
		        timeEntryCollectionService.Add(item);
	        }


            //timeEntryCollectionService.Add(new Model.TimeEntry()
            //{
            //    description = "144449:6.4.0.4 - Set system context on login into Student Self-Management and Learner Portals",
            //    start = new DateTime(2014, 2, 11),
            //    duration = 20284,
            //});

            //timeEntryCollectionService.Add(new Model.TimeEntry()
            //{
            //    description = "Stand Up",
            //    start = new DateTime(2014, 2, 11),
            //    duration = 611,
            //});

            //timeEntryCollectionService.Add(new Model.TimeEntry()
            //{
            //    description = "144269:6.4.4.2 Apps In Client: Limit editing of a Learner's applications where context doesn't match",
            //    start = new DateTime(2014, 2, 11),
            //    duration = 8298,
            //});
        }
    }
}
