using LittleRestClient;
using Moq.Protected;
using Moq;
using System.Net;

namespace LittleRestClient.UnitTestProject;

[TestClass]
public class GetStringAsyncTests
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
    public async Task GetStringAsync_Happy_Test()
    {
        // Arrange
        var testString = "Test String";
        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == new Uri("https://test.com/TestString")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(testString),
            });

        // Act
        var response = await _restClient.GetStringAsync("TestString");

        // Assert
        Assert.AreEqual(testString, response);
    }

    [TestMethod]
    public async Task GetStringAsync_NotFound_Test()
    {
        // Arrange
        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == new Uri("https://test.com/TestString")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent("Not Found"),
            });

        // Act & Assert
        var exception = await Assert.ThrowsExceptionAsync<HttpRequestException>(() => _restClient.GetStringAsync("TestString"));
        Assert.IsTrue(exception.Message.Contains("Not Found"));
    }

    [TestMethod]
    public async Task GetStringAsync_ServerError_Test()
    {
        // Arrange
        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == new Uri("https://test.com/TestString")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Internal Server Error"),
            });

        // Act & Assert
        var exception = await Assert.ThrowsExceptionAsync<HttpRequestException>(() => _restClient.GetStringAsync("TestString"));
        Assert.IsTrue(exception.Message.Contains("Internal Server Error"));
    }

    [TestMethod]
    public async Task GetStringAsync_EmptyResponse_Test()
    {
        // Arrange
        var emptyContent = string.Empty;
        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == new Uri("https://test.com/TestString")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(emptyContent),
            });

        // Act
        var response = await _restClient.GetStringAsync("TestString");

        // Assert
        Assert.AreEqual(emptyContent, response);
    }
}
