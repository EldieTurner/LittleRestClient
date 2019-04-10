using System.Net;
using System.Net.Http;
using System.Threading;
using LittleRestClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTestProject
{
    [TestClass]
    public class GetAsync_Tests
    {
        [TestMethod]
        public void GetAsync_Happy_Test()
        {
            //Arrange
            var testObject = new SimpleTestObject { TestProperty = "Test Value", TestProperty2 = 2 };
            var content = new StringContent(testObject.ToJsonString());
            var httpClientResponse = new HttpResponseMessage(HttpStatusCode.OK){Content = content};
            var httpClient = new Mock<IHttpClient>();
            httpClient.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpClientResponse);
            var config = new TestRestConfig();
            var client = new TestRestClient(config, httpClient.Object);

            //Act
            var responseTask = client.GetAsync<SimpleTestObject>("TestObject");
            var response = responseTask.GetAwaiter().GetResult();
            
            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(response.Data.TestProperty, testObject.TestProperty);
        }

        [TestMethod]
        public void GetAsync_Sad_Test()
        {   
            //Arrange
            var httpClientResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var httpClient = new Mock<IHttpClient>();
            httpClient.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpClientResponse);
            var config = new TestRestConfig();
            var client = new TestRestClient(config, httpClient.Object);

            //Act
            var responseTask = client.GetAsync<SimpleTestObject>("TestObject");
            var response = responseTask.GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(response.IsSuccessStatusCode);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
            Assert.IsNull(response.Data);
        }
    }
}