using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GoalLine.Structures;

namespace GoalLine.UnitTests
{
    [TestClass]
    public class PersonTests
    {
        Person StandardPerson;

        public PersonTests()
        {
            StandardPerson = new Person();
            StandardPerson.FirstName = "Michael";
            StandardPerson.LastName = "Knight";
        }

        [TestMethod]
        public void TestNameOutput_FirstLast()
        {
            string Result = StandardPerson.DisplayName(PersonNameReturnType.FirstnameLastname);
            Assert.AreEqual("Michael Knight", Result);
        }

        [TestMethod]
        public void TestNameOutput_InitialLast()
        {
            string Result = StandardPerson.DisplayName(PersonNameReturnType.InitialLastname);
            Assert.AreEqual("M. Knight", Result);
        }

        [TestMethod]
        public void TestNameOutput_InitialOptionalLast_NoInitial()
        {
            StandardPerson.UseInitial = false;
            string Result = StandardPerson.DisplayName(PersonNameReturnType.InitialOptionalLastname);
            Assert.AreEqual("Knight", Result);
        }

        [TestMethod]
        public void TestNameOutput_InitialOptionalLast_WithInitial()
        {
            StandardPerson.UseInitial = true;
            string Result = StandardPerson.DisplayName(PersonNameReturnType.InitialOptionalLastname);
            Assert.AreEqual("M. Knight", Result);
        }

        [TestMethod]
        public void TestNameOutput_LastFirst()
        {
            string Result = StandardPerson.DisplayName(PersonNameReturnType.LastnameFirstname);
            Assert.AreEqual("Knight, Michael", Result);
        }

        [TestMethod]
        public void TestNameOutput_LastInitial()
        {
            string Result = StandardPerson.DisplayName(PersonNameReturnType.LastnameInitial);
            Assert.AreEqual("Knight, M.", Result);
        }

        [TestMethod]
        public void TestNameOutput_LastInitialOptional_NoInitial()
        {
            StandardPerson.UseInitial = false;
            string Result = StandardPerson.DisplayName(PersonNameReturnType.LastnameInitialOptional);
            Assert.AreEqual("Knight", Result);
        }

        [TestMethod]
        public void TestNameOutput_LastInitialOptional_WithInitial()
        {
            StandardPerson.UseInitial = true;
            string Result = StandardPerson.DisplayName(PersonNameReturnType.LastnameInitialOptional);
            Assert.AreEqual("Knight, M.", Result);
        }
    }
}
