using Moq.Protected;
using Moq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace LittleRestClient.UnitTestProject;

[TestClass]
public class PutAsyncTests
{
    private Mock<HttpMessageHandler> _handlerMock;
    private HttpClient _httpClient;
    private RestClient _restClient;

    [TestInitialize]
    public void Setup()
    {
        _handlerMock = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new Uri("https://test.com/")
        };

        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(_httpClient);

        var config = new TestRestConfig(); // Assuming you have a TestRestConfig class implementing IRestClientConfig
        _restClient = new RestClient(config, httpClientFactoryMock.Object);
    }

    [TestMethod]
    public async Task PutAsync_Happy_Test()
    {
        // Arrange
        var requestObject = new SimpleTestObject { TestProperty = "Request Value", TestProperty2 = 1 };
        var responseObject = new SimpleTestObject { TestProperty = "Response Value", TestProperty2 = 2 };
        var jsonResponse = JsonSerializer.Serialize(responseObject);
        var responseContent = new StringContent(jsonResponse, Encoding.UTF8, "application/json");

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put &&
                    req.RequestUri == new Uri("https://test.com/TestRoute")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = responseContent,
            });

        // Act
        var response = await _restClient.PutAsync<SimpleTestObject, SimpleTestObject>("TestRoute", requestObject);

        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode);
        Assert.AreEqual(responseObject.TestProperty, response.Data.TestProperty);
        Assert.AreEqual(responseObject.TestProperty2, response.Data.TestProperty2);
    }

    [TestMethod]
    public async Task PutAsync_NotFound_Test()
    {
        // Arrange
        var requestObject = new SimpleTestObject { TestProperty = "Request Value", TestProperty2 = 1 };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put &&
                    req.RequestUri == new Uri("https://test.com/TestRoute")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Not Found"),
            });

        // Act
        var response = await _restClient.PutAsync<SimpleTestObject, SimpleTestObject>("TestRoute", requestObject);

        // Assert
        Assert.IsFalse(response.IsSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        Assert.IsNull(response.Data);
    }

    [TestMethod]
    public async Task PutAsync_ServerError_Test()
    {
        // Arrange
        var requestObject = new SimpleTestObject { TestProperty = "Request Value", TestProperty2 = 1 };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put &&
                    req.RequestUri == new Uri("https://test.com/TestRoute")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Internal Server Error"),
            });

        // Act
        var response = await _restClient.PutAsync<SimpleTestObject, SimpleTestObject>("TestRoute", requestObject);

        // Assert
        Assert.IsFalse(response.IsSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.IsNull(response.Data);
    }

    [TestMethod]
    public async Task PutAsync_EmptyResponse_Test()
    {
        // Arrange
        var requestObject = new SimpleTestObject { TestProperty = "Request Value", TestProperty2 = 1 };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put &&
                    req.RequestUri == new Uri("https://test.com/TestRoute")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(string.Empty),
            });

        // Act
        var response = await _restClient.PutAsync<SimpleTestObject, SimpleTestObject>("TestRoute", requestObject);

        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode);
        Assert.IsNull(response.Data);
    }
}
