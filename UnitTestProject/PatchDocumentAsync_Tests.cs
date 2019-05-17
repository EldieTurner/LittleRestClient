using System.Net;
using System.Net.Http;
using System.Threading;
using LittleRestClient;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTestProject
{
    [TestClass]
    public class PatchDocumentAsync_Tests
    {
        [TestMethod]
        public void PatchAsync_Happy_Test()
        {
            //Arrange
            var testObject = new SimpleTestObject { TestProperty = "Test Value", TestProperty2 = 2 };
            var content = new StringContent(testObject.ToJsonString());
            var httpClientResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
            var httpClient = new Mock<IHttpClient>();
            httpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpClientResponse);
            var config = new TestRestConfig();
            var restClient = new TestRestClient(config, httpClient.Object);
            string newValue = nameof(newValue);
            var doc = new JsonPatchDocument<SimpleTestObject>();
            doc.Replace(x => x.TestProperty, newValue);

            //Act
            var responseTask = restClient.PatchAsync<SimpleTestObject, SimpleTestObject>("TestObject", doc);
            var response = responseTask.GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            httpClient.Verify(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public void PatchAsync_Sad_Test()
        {
            //Arrange
            var testObject = new SimpleTestObject { TestProperty = "Test Value", TestProperty2 = 2 };
            var httpClientResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = null };
            var httpClient = new Mock<IHttpClient>();
            httpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpClientResponse);
            var config = new TestRestConfig();
            var restClient = new TestRestClient(config, httpClient.Object);
            string newValue = nameof(newValue);
            var doc = new JsonPatchDocument<SimpleTestObject>();
            doc.Replace(x => x.TestProperty, newValue);


            //Act
            var responseTask = restClient.PatchAsync<SimpleTestObject, SimpleTestObject>("TestObject", doc);
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
            httpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpClientResponse);
            var config = new TestRestConfig();
            var restClient = new TestRestClient(config, httpClient.Object);
            string newValue = nameof(newValue);
            var doc = new JsonPatchDocument<SimpleTestObject>();
            doc.Replace(x => x.TestProperty, newValue);

            //Act
            var responseTask = restClient.PatchAsync("TestObject", doc);
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
            httpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(httpClientResponse);
            var config = new TestRestConfig();
            var restClient = new TestRestClient(config, httpClient.Object);
            string newValue = nameof(newValue);
            var doc = new JsonPatchDocument<SimpleTestObject>();
            doc.Replace(x => x.TestProperty, newValue);

            //Act
            var responseTask = restClient.PatchAsync("TestObject", doc);
            var response = responseTask.GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(response.IsSuccessStatusCode);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.InternalServerError);
        }

    }
}
