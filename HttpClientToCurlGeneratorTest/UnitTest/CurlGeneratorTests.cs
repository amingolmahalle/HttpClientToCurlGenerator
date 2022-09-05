using System.Text;
using HttpClientToCurl;
using Microsoft.AspNetCore.WebUtilities;
using NUnit.Framework;

namespace HttpClientToCurlGeneratorTest.UnitTest;

public class CurlGeneratorTests
{
    #region :: GenerateCurl_For_PostMethod ::

    [Theory]
    public void Success_GenerateCurl_With_QueryString_For_PostMethod()
    {
        // Arrange
        string requestBody = @"""{ ""name"" : ""amin"",""requestId"" : 10001000,""amount"":10000 }""";
        var queryString = new Dictionary<string, string>()
        {
            { "id", "12" }
        };
        var requestUri = QueryHelpers.AddQueryString("api/test", queryString);
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
        httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        httpRequestMessage.Headers.Add("Authorization", "4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213");

        // Act
        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            requestUri,
            true);

        // Assert
        Assert.That(script, Is.Not.Null);
        Assert.That(script, Is.Not.Empty);
        Assert.That(script, Does.StartWith("curl"));
        Assert.That(script?.Trim(),
            Is.EqualTo(
                @"curl -X POST http://localhost:1213/api/test?id=12 -H 'Authorization: 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d '""{ ""name"" : ""amin"",""requestId"" : 10001000,""amount"":10000 }""'"));
    }

    [Theory]
    public void Success_GenerateCurl_Without_QueryString_For_PostMethod()
    {
        // Arrange
        string requestBody = @"""{ ""name"" : ""sara"",""requestId"" : 10001001,""amount"":20000 }""";

        var requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
        httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        httpRequestMessage.Headers.Add("Authorization", "4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213");

        // Act
        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            requestUri,
            true);

        // Assert
        Assert.That(script, Is.Not.Null);
        Assert.That(script, Is.Not.Empty);
        Assert.That(script, Does.StartWith("curl"));
        Assert.That(script?.Trim(),
            Is.EqualTo(
                @"curl -X POST http://localhost:1213/api/test -H 'Authorization: 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d '""{ ""name"" : ""sara"",""requestId"" : 10001001,""amount"":20000 }""'"));
    }

    #endregion

    #region :: GenerateCurl_For_GetMethod ::

    [Theory]
    public void Success_GenerateCurl_With_QueryString_For_GetMethod()
    {
        // Arrange
        var queryString = new Dictionary<string, string>()
        {
            { "id", "12" }
        };
        var requestUri = QueryHelpers.AddQueryString("api/test", queryString);
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
        httpRequestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
        httpRequestMessage.Headers.Add("Authorization", "703438f3-16ad-4ba5-b923-8f72cd0f2db9");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213");

        // Act
        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            requestUri,
            true);

        // Assert
        Assert.That(script, Is.Not.Null);
        Assert.That(script, Is.Not.Empty);
        Assert.That(script, Does.StartWith("curl"));
        Assert.That(script?.Trim(),
            Is.EqualTo(@"curl http://localhost:1213/api/test?id=12 -H 'Authorization: 703438f3-16ad-4ba5-b923-8f72cd0f2db9' -H 'Content-Type: application/json; charset=utf-8'"));
    }

    [Theory]
    public void Success_GenerateCurl_Without_QueryString_For_GetMethod()
    {
        // Arrange

        var requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
        httpRequestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
        httpRequestMessage.Headers.Add("Authorization", "703438f3-16ad-4ba5-b923-8f72cd0f2db9");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213");

        // Act
        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            requestUri,
            true);

        // Assert
        Assert.That(script, Is.Not.Null);
        Assert.That(script, Is.Not.Empty);
        Assert.That(script, Does.StartWith("curl"));
        Assert.That(script?.Trim(),
            Is.EqualTo(@"curl http://localhost:1213/api/test -H 'Authorization: 703438f3-16ad-4ba5-b923-8f72cd0f2db9' -H 'Content-Type: application/json; charset=utf-8'"));
    }

    #endregion

    #region :: GenerateCurl_For_PutMethod ::

    [Theory]
    public void Success_GenerateCurl_For_PutMethod()
    {
        // Arrange
        string requestBody = @"""{ ""name"" : ""reza"",""requestId"" : 10001002,""amount"":30000 }""";

        var requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri);
        httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        httpRequestMessage.Headers.Add("Authorization", "4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213");

        // Act
        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            requestUri,
            true);

        // Assert
        Assert.That(script, Is.Not.Null);
        Assert.That(script, Is.Not.Empty);
        Assert.That(script, Does.StartWith("curl"));
        Assert.That(script?.Trim(),
            Is.EqualTo(
                @"curl -X PUT http://localhost:1213/api/test -H 'Authorization: 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d '""{ ""name"" : ""reza"",""requestId"" : 10001002,""amount"":30000 }""'"));
    }

    #endregion

    #region :: GenerateCurl_For_PatchMethod ::

    [Theory]
    public void Success_GenerateCurl_For_PatchMethod()
    {
        // Arrange
        string requestBody = @"""{ ""name"" : ""hamed"",""requestId"" : 10001003,""amount"":40000 }""";

        var requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, requestUri);
        httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
        httpRequestMessage.Headers.Add("Authorization", "4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213");

        // Act
        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            requestUri,
            true);

        // Assert
        Assert.That(script, Is.Not.Null);
        Assert.That(script, Is.Not.Empty);
        Assert.That(script, Does.StartWith("curl"));
        Assert.That(script?.Trim(),
            Is.EqualTo(
                @"curl -X PATCH http://localhost:1213/api/test -H 'Authorization: 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d '""{ ""name"" : ""hamed"",""requestId"" : 10001003,""amount"":40000 }""'"));
    }

    #endregion

    #region :: GenerateCurl_For_DeleteMethod ::

    [Theory]
    public void Success_GenerateCurl_For_DeleteMethod()
    {
        // Arrange
        var id = 12;
        var requestUri = $"api/test/{id}";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, requestUri);
        httpRequestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
        httpRequestMessage.Headers.Add("Authorization", "703438f3-16ad-4ba5-b923-8f72cd0f2db9");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213");

        // Act
        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            requestUri,
            true);

        // Assert
        Assert.That(script, Is.Not.Null);
        Assert.That(script, Is.Not.Empty);
        Assert.That(script, Does.StartWith("curl"));
        Assert.That(script?.Trim(),
            Is.EqualTo(
                @"curl -X DELETE http://localhost:1213/api/test/12 -H 'Authorization: 703438f3-16ad-4ba5-b923-8f72cd0f2db9' -H 'Content-Type: application/json; charset=utf-8'"));
    }

    #endregion
}