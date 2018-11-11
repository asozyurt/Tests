using HeadlessBrowser.Common.Dto;
using HeadlessBrowser.Common.Message;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HeadlessBrowser.TestRunner
{
    [TestClass]
    public class NameDbTests
    {
        [TestMethod]
        public void SelectExistingRecordTest()
        {
            long recordId = 1048;
            var expectedName = "Giselle";

            ResponseMessage response = Service.NameServices.GetNameById(recordId);

            Assert.IsFalse(response.IsError);
            Assert.IsInstanceOfType(response.Result, typeof(NameDto));
            Assert.AreEqual(expectedName, ((NameDto)response.Result).Name);
        }

        [TestMethod]
        public void SelectNonExistingRecordTest()
        {
            long recordId = 15925846;
            ResponseMessage response = Service.NameServices.GetNameById(recordId);

            Assert.IsTrue(response.IsError);
            Assert.IsNull(response.Result);
        }

        [TestMethod]
        public void FindByNameTest()
        {
            string queryName = "Giselle";
            long expectedId = 1048;

            ResponseMessage response = Service.NameServices.FindName(queryName);

            Assert.IsFalse(response.IsError);
            Assert.IsInstanceOfType(response.Result, typeof(NameDto));
            Assert.AreEqual(expectedId, ((NameDto)response.Result).Id);
        }

        [TestMethod]
        public void InsertTest()
        {
            NameDto newRecord = new NameDto
            {
                Name = "TestDummyName"
            };

            ResponseMessage response = Service.NameServices.Insert(newRecord);

            long notExpected = 0L;
            Assert.IsFalse(response.IsError);
            Assert.IsNotNull(response.Result);
            Assert.IsInstanceOfType(response.Result, typeof(NameDto));
            Assert.AreNotSame(notExpected, ((NameDto)response.Result) .Id);
        }

        [TestMethod]
        public void UpdateTest()
        {
            NameDto newRecord = new NameDto
            {
                Name = "TestDummyNameUpdated"
            };

            ResponseMessage response = Service.NameServices.Update(newRecord);

            string expected = "TestDummyNameUpdated";
            Assert.IsFalse(response.IsError);
            Assert.IsNotNull(response.Result);
            Assert.IsInstanceOfType(response.Result, typeof(NameDto));
            Assert.AreNotSame(expected, ((NameDto)response.Result).Name);
        }

        [TestMethod]
        public void DeleteTest()
        {
        }

        [TestMethod]
        public void HardDeleteTest()
        {
        }
    }
}
