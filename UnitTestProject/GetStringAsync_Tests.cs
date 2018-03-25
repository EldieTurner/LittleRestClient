using System.Net;
using System.Net.Http;
using LittleRestClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTestProject
{
    [TestClass]
    public class GetStringAsync_Tests
    {
        [TestMethod]
        public void GetStringAsync_Happy_Test()
        {
            //Arrange
            var testObject = new SimpleTestObject { TestProperty = "Test Value", TestProperty2 = 2 };
            var httpClient = new Mock<IHttpClient>();
            httpClient.Setup(x => x.GetStringAsync(It.IsAny<string>()))
                .ReturnsAsync(testObject.ToJsonString());
            var config = new TestRestConfig();
            var client = new TestRestClient(config, httpClient.Object);

            //Act
            var responseTask = client.GetStringAsync("TestObject");
            var response = responseTask.GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual(response, testObject.ToJsonString());
        }
    }
}
