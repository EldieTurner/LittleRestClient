using System.Net;
using System.Net.Http;
using System.Text;
using LittleRestClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace UnitTestProject
{
    [TestClass]
    public class PatchAsync_Tests
    {
        [TestMethod]
        public void PatchAsync_Happy_Test()
        {
            //Arrange
            var testObject = new SimpleTestObject { TestProperty = "Test Value", TestProperty2 = 2 };
            var content = new StringContent(testObject.ToJsonString());
            var httpClientResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
            var httpClient = new Mock<IHttpClient>();
            httpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(httpClientResponse);
            var config = new TestRestConfig();
            var restClient = new TestRestClient(config, httpClient.Object);

            //Act
            var responseTask = restClient.PatchAsync<SimpleTestObject, SimpleTestObject>("TestObject", testObject);
            var response = responseTask.GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(response.Data.TestProperty, testObject.TestProperty);
        }

        [TestMethod]
        public void PatchAsync_Sad_Test()
        {
            //Arrange
            var testObject = new SimpleTestObject { TestProperty = "Test Value", TestProperty2 = 2 };
            var httpClientResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = null };
            var httpClient = new Mock<IHttpClient>();
            httpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(httpClientResponse);
            var config = new TestRestConfig();
            var restClient = new TestRestClient(config, httpClient.Object);

            //Act
            var responseTask = restClient.PatchAsync<SimpleTestObject, SimpleTestObject>("TestObject", testObject);
            var response = responseTask.GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(response.IsSuccessStatusCode);
            Assert.IsNull(response.Data);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }

        [TestMethod]
        public void PatchAsync_No_Response_Happy_Test()
        {
            //Arrange
            var testObject = new SimpleTestObject { TestProperty = "Test Value", TestProperty2 = 2 };
            var httpClientResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var httpClient = new Mock<IHttpClient>();
            httpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(httpClientResponse);
            var config = new TestRestConfig();
            var restClient = new TestRestClient(config, httpClient.Object);

            //Act
            var responseTask = restClient.PatchAsync("TestObject", testObject);
            var response = responseTask.GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void PatchAsync_No_Response_Sad_Test()
        {
            //Arrange
            var testObject = new SimpleTestObject { TestProperty = "Test Value", TestProperty2 = 2 };
            var httpClientResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var httpClient = new Mock<IHttpClient>();
            httpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(httpClientResponse);
            var config = new TestRestConfig();
            var restClient = new TestRestClient(config, httpClient.Object);

            //Act
            var responseTask = restClient.PatchAsync("TestObject", testObject);
            var response = responseTask.GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(response.IsSuccessStatusCode);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }
    }
}
