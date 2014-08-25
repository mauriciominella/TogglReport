using Ahgora.Api.Model;
using Ahgora.Api.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ahgora.Api.Tests.Repository
{
    [TestClass]
    public class TimesheetRepositoryTests
    {
        [TestMethod]
        public void Should_Retreive_A_Not_Null_RootObject()
        {
            TimesheetRepository timesheetRepository = new TimesheetRepository(string.Empty, string.Empty);
            RootObject timesheetCollection = timesheetRepository.GetAll();
            Assert.IsNotNull(timesheetCollection);

        }

        [TestMethod]
        public void Should_Set_Username_Property_When_Its_Passed_On_Constructor()
        {
            string username = "username";
            TimesheetRepository timesheetRepository = new TimesheetRepository(username, string.Empty);
            Assert.AreEqual(username, timesheetRepository.Username);
        }

        [TestMethod]
        public void Should_Set_Password_Property_When_Its_Passed_On_Constructor()
        {
            string password = "pass";
            TimesheetRepository timesheetRepository = new TimesheetRepository(string.Empty, password);
            Assert.AreEqual(password, timesheetRepository.Password);
        }

        [TestMethod]
        public void Should_Retrieve_An_List_Greater_Than_Zero_When_User_Is_Valid()
        {
            string validUsername = "35";
            string validPassword = "3006";

            TimesheetRepository timesheetRepository = new TimesheetRepository(validUsername, validPassword);
            RootObject timesheetCollection = timesheetRepository.GetAll();
            Assert.IsTrue(timesheetCollection.timesheet.Count > 0);
        }
    }
}
