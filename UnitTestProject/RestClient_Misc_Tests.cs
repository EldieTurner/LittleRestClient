using LittleRestClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace UnitTestProject
{
    [TestClass]
    public class RestClient_Misc_Tests
    {
        [TestMethod]
            public void Empty_Url_Test()
        {
            Assert.ThrowsException<ArgumentException>(() => new RestClient(""));
        }

        [TestMethod]
        public void Empty_Config_Test()
        {
            var config = new Mock<IRestClientConfig>();
            Assert.ThrowsException<ArgumentException>(() => new RestClient(config.Object));
        }

        [TestMethod]
        public void Simple_Constructor_Test()
        {
            //Arrange
            var url = "http://example.com/api";

            //Act
            var client = new TestRestClient(url);

            //Assert
            Assert.AreEqual(url, client.HttpClient.BaseAddress.ToString());
        }

        [TestMethod]
        public void Standard_Constructor_Test()
        {
            //Arrange
            var config = new TestRestConfig {BaseUrl = "http://example.com/api"};

            //Act
            var client = new TestRestClient(config);

            //Assert
            Assert.AreEqual(config.BaseUrl, client.HttpClient.BaseAddress.ToString());
        }
    }
}
