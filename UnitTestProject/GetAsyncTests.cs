using System.Net;
using System.Text;
using System.Text.Json;
using Moq;
using Moq.Protected;

namespace LittleRestClient.UnitTestProject;
[TestClass]
public class GetAsyncTests
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

        IRestClientConfig config = new TestRestConfig(); // Assuming you have a TestRestConfig class implementing IRestClientConfig
        _restClient = new RestClient(config, httpClientFactoryMock.Object);
    }

    [TestMethod]
    public async Task GetAsync_Happy_Test()
    {
        // Arrange
        var testObject = new SimpleTestObject { TestProperty = "Test Value", TestProperty2 = 2 };
        var jsonContent = JsonSerializer.Serialize(testObject);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == new Uri("https://test.com/TestObject")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = content,
            });

        // Act
        var response = await _restClient.GetAsync<SimpleTestObject>("TestObject");

        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode);
        Assert.AreEqual(testObject.TestProperty, response.Data.TestProperty);
        Assert.AreEqual(testObject.TestProperty2, response.Data.TestProperty2);
    }

    [TestMethod]
    public async Task GetAsync_NotFound_Test()
    {
        // Arrange
        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == new Uri("https://test.com/TestObject")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            });

        // Act
        var response = await _restClient.GetAsync<SimpleTestObject>("TestObject");

        // Assert
        Assert.IsFalse(response.IsSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        Assert.IsNull(response.Data);
    }

    [TestMethod]
    public async Task GetAsync_ServerError_Test()
    {
        // Arrange
        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == new Uri("https://test.com/TestObject")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        // Act
        var response = await _restClient.GetAsync<SimpleTestObject>("TestObject");

        // Assert
        Assert.IsFalse(response.IsSuccessStatusCode);
        Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.IsNull(response.Data);
    }

    [TestMethod]
    public async Task GetAsync_MalformedJson_Test()
    {
        // Arrange
        var malformedJsonContent = "{ 'TestProperty': 'Test Value', 'TestProperty2': 'NotAnInt' }";
        var content = new StringContent(malformedJsonContent, Encoding.UTF8, "application/json");

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == new Uri("https://test.com/TestObject")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = content,
            });

        // Act and Assert
        await Assert.ThrowsExceptionAsync<JsonException>(async () =>
        {
            await _restClient.GetAsync<SimpleTestObject>("TestObject");
        });
    }

    [TestMethod]
    public async Task GetAsync_EmptyResponse_Test()
    {
        // Arrange
        var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == new Uri("https://test.com/TestObject")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = content,
            });

        // Act
        var response = await _restClient.GetAsync<SimpleTestObject>("TestObject");

        // Assert
        Assert.IsTrue(response.IsSuccessStatusCode);
        Assert.IsNull(response.Data);
    }
}