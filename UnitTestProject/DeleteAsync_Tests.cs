using System.Net;
using System.Net.Http;
using System.Threading;
using LittleRestClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTestProject
{
    [TestClass]
    public class DeleteAsync_Tests
    {
        [TestMethod]
        public void DeleteAsync_Happy_Test()
        {
            //Arrange
            var testObject = new SimpleTestObject { TestProperty = "Test Value", TestProperty2 = 2 };
            var content = new StringContent(testObject.ToJsonString());
            var httpClientResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
            var httpClient = new Mock<IHttpClient>();
            httpClient.Setup(x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpClientResponse);
            var config = new TestRestConfig();
            var client = new TestRestClient(config, httpClient.Object);

            //Act
            var responseTask = client.DeleteAsync<SimpleTestObject>("TestObject");
            var response = responseTask.GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(response.Data.TestProperty, testObject.TestProperty);
        }

        [TestMethod]
        public void DeleteAsync_Sad_Test()
        {
            //Arrange
            var httpClientResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var httpClient = new Mock<IHttpClient>();
            httpClient.Setup(x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpClientResponse);
            var config = new TestRestConfig();
            var client = new TestRestClient(config, httpClient.Object);

            //Act
            var responseTask = client.DeleteAsync<SimpleTestObject>("TestObject");
            var response = responseTask.GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(response.IsSuccessStatusCode);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
            Assert.IsNull(response.Data);
        }

        [TestMethod]
        public void DeleteAsync_No_Response_Happy_Test()
        {
            //Arrange
            var testObject = new SimpleTestObject { TestProperty = "Test Value", TestProperty2 = 2 };
            var httpClientResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var httpClient = new Mock<IHttpClient>();
            httpClient.Setup(x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpClientResponse);
            var config = new TestRestConfig();
            var client = new TestRestClient(config, httpClient.Object);

            //Act
            var responseTask = client.DeleteAsync("TestObject");
            var response = responseTask.GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void DeleteAsync_No_Response_Sad_Test()
        {
            //Arrange
            var httpClientResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var httpClient = new Mock<IHttpClient>();
            httpClient.Setup(x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpClientResponse);
            var config = new TestRestConfig();
            var client = new TestRestClient(config, httpClient.Object);

            //Act
            var responseTask = client.DeleteAsync("TestObject");
            var response = responseTask.GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(response.IsSuccessStatusCode);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }
    }
}
