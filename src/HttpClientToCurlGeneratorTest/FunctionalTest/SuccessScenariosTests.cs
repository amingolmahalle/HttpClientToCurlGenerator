using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using HttpClientToCurl;
using Microsoft.AspNetCore.WebUtilities;
using NUnit.Framework;

namespace HttpClientToCurlGeneratorTest.FunctionalTest;

public class SuccessScenariosTests
{
    #region :: GenerateCurlInString For Post Method ::

    [Theory]
    public void Success_GenerateCurlInString_For_PostMethod()
    {
        // Arrange
        string requestBody = @"{""name"":""sara"",""requestId"":10001001,""amount"":20000}";

        var requestUri = "/api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
        httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json);
        httpRequestMessage.Headers.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        Assert.That(curlResult, Is.Not.Null);
        Assert.That(curlResult, Is.Not.Empty);
        Assert.That(curlResult, Does.StartWith("curl -X POST"));
        Assert.That(curlResult?.Trim(),
            Is.EqualTo(
                @"curl -X POST http://localhost:1213/v1/api/test -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d '{""name"":""sara"",""requestId"":10001001,""amount"":20000}'"));
    }

    [Theory]
    public void GenerateCurl_When_Set_RequestUri_Inside_HttpRequestMessage_For_PostMethod()
    {
        // Arrange
        string requestBody = @"{""name"":""sara"",""requestId"":10001001,""amount"":20000}";

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "/api/test");
        httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json);
        httpRequestMessage.Headers.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        Assert.That(curlResult, Is.Not.Null);
        Assert.That(curlResult, Is.Not.Empty);
        Assert.That(curlResult, Does.StartWith("curl -X POST"));
        Assert.That(curlResult?.Trim(),
            Is.EqualTo(
                @"curl -X POST http://localhost:1213/v1/api/test -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d '{""name"":""sara"",""requestId"":10001001,""amount"":20000}'"));
    }

    [Theory]
    public void Success_GenerateCurlInString_When_RequestUri_Is_Null_For_PostMethod()
    {
        // Arrange
        string requestBody = @"{""name"":""sara"",""requestId"":10001001,""amount"":20000}";

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "");
        httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json);
        httpRequestMessage.Headers.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        Assert.That(curlResult, Is.Not.Null);
        Assert.That(curlResult, Is.Not.Empty);
        Assert.That(curlResult, Does.StartWith("curl -X POST"));
        Assert.That(curlResult?.Trim(),
            Is.EqualTo(
                @"curl -X POST http://localhost:1213/v1 -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d '{""name"":""sara"",""requestId"":10001001,""amount"":20000}'"));
    }

    [Theory]
    public void Success_GenerateCurlInString_When_RequestBody_Is_Null_For_PostMethod()
    {
        // Arrange
        var requestUri = "/api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
        httpRequestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);
        httpRequestMessage.Headers.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        Assert.That(curlResult, Is.Not.Null);
        Assert.That(curlResult, Is.Not.Empty);
        Assert.That(curlResult, Does.StartWith("curl -X POST"));
        Assert.That(curlResult?.Trim(),
            Is.EqualTo(
                @"curl -X POST http://localhost:1213/v1/api/test -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d ''"));
    }

    [Theory]
    public void Success_GenerateCurlInString_When_HttpContent_Is_Null_For_PostMethod()
    {
        // Arrange
        var requestUri = "/api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
        httpRequestMessage.Headers.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        Assert.That(curlResult, Is.Not.Null);
        Assert.That(curlResult, Is.Not.Empty);
        Assert.That(curlResult, Does.StartWith("curl -X POST"));
        Assert.That(curlResult?.Trim(),
            Is.EqualTo(
                @"curl -X POST http://localhost:1213/v1/api/test -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -d ''"));
    }

    [Theory]
    public void Success_GenerateCurlInString_Without_HttpRequestMessage_For_PostMethod()
    {
        // Arrange
        var requestObject = new
        {
            name = "sara",
            requestId = 10001001,
            amount = 20000,
        };

        JsonContent jsonContent = JsonContent.Create(requestObject);

        var requestUri = "/api/test";
        HttpRequestHeaders httpRequestHeaders = new HttpRequestMessage().Headers;
        httpRequestHeaders.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1");

        // Act
        string curlResult = httpClient.GenerateCurlInString(
            HttpMethod.Post, requestUri, httpRequestHeaders, jsonContent);

        // Assert
        Assert.That(curlResult, Is.Not.Null);
        Assert.That(curlResult, Is.Not.Empty);
        Assert.That(curlResult, Does.StartWith("curl -X POST"));
        Assert.That(curlResult?.Trim(),
            Is.EqualTo(
                @"curl -X POST http://localhost:1213/v1/api/test -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d '{""name"":""sara"",""requestId"":10001001,""amount"":20000}'"));
    }

    [Theory]
    public void Success_GenerateCurlInString_Without_HttpRequestMessage_And_Body_Is_Null_For_PostMethod()
    {
        // Arrange
        var requestUri = "/api/test";
        HttpRequestHeaders httpRequestHeaders = new HttpRequestMessage().Headers;
        httpRequestHeaders.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1");

        // Act
        string curlResult = httpClient.GenerateCurlInString(
            HttpMethod.Post, requestUri, httpRequestHeaders);

        // Assert
        Assert.That(curlResult, Is.Not.Null);
        Assert.That(curlResult, Is.Not.Empty);
        Assert.That(curlResult, Does.StartWith("curl -X POST"));
        Assert.That(curlResult?.Trim(),
            Is.EqualTo(
                @"curl -X POST http://localhost:1213/v1/api/test -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -d ''"));
    }

    [Theory]
    public void Success_GenerateCurlInString_Without_HttpRequestMessage_And_RequestUri_Is_Null_For_PostMethod()
    {
        // Arrange
        var requestObject = new
        {
            name = "sara",
            requestId = 10001001,
            amount = 20000,
        };

        JsonContent jsonContent = JsonContent.Create(requestObject);

        HttpRequestHeaders httpRequestHeaders = new HttpRequestMessage().Headers;
        httpRequestHeaders.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1");

        // Act
        string curlResult = httpClient.GenerateCurlInString(
            httpMethod: HttpMethod.Post, requestHeaders: httpRequestHeaders, requestBody: jsonContent);

        // Assert
        Assert.That(curlResult, Is.Not.Null);
        Assert.That(curlResult, Is.Not.Empty);
        Assert.That(curlResult, Does.StartWith("curl -X POST"));
        Assert.That(curlResult?.Trim(),
            Is.EqualTo(
                @"curl -X POST http://localhost:1213/v1 -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d '{""name"":""sara"",""requestId"":10001001,""amount"":20000}'"));
    }

    [Theory]
    public void Success_GenerateCurlInString_Without_HttpRequestMessage_And_HttpRequestHeader_Is_null_For_PostMethod()
    {
        // Arrange
        var requestObject = new
        {
            name = "sara",
            requestId = 10001001,
            amount = 20000,
        };

        JsonContent jsonContent = JsonContent.Create(requestObject);

        var requestUri = "/api/test";

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1");

        // Act
        string curlResult = httpClient.GenerateCurlInString(
            httpMethod: HttpMethod.Post, requestUri: requestUri, requestBody: jsonContent);

        // Assert
        Assert.That(curlResult, Is.Not.Null);
        Assert.That(curlResult, Is.Not.Empty);
        Assert.That(curlResult, Does.StartWith("curl -X POST"));
        Assert.That(curlResult?.Trim(),
            Is.EqualTo(
                @"curl -X POST http://localhost:1213/v1/api/test -H 'Content-Type: application/json; charset=utf-8' -d '{""name"":""sara"",""requestId"":10001001,""amount"":20000}'"));
    }

    #endregion

    #region :: GenerateCurlInString For Get Method ::

    [Theory]
    public void Success_GenerateCurlInString_For_GetMethod()
    {
        // Arrange
        var requestUri = "/api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
        httpRequestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);
        httpRequestMessage.Headers.Add("Authorization", "Bearer 703438f3-16ad-4ba5-b923-8f72cd0f2db9");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        Assert.That(curlResult, Is.Not.Null);
        Assert.That(curlResult, Is.Not.Empty);
        Assert.That(curlResult, Does.StartWith("curl"));
        Assert.That(curlResult?.Trim(),
            Is.EqualTo(@"curl http://localhost:1213/v1/api/test -H 'Authorization: Bearer 703438f3-16ad-4ba5-b923-8f72cd0f2db9' -H 'Content-Type: application/json; charset=utf-8'"));
    }

    [Theory]
    public void Success_GenerateCurlInString_With_QueryString_For_GetMethod()
    {
        // Arrange
        var queryString = new Dictionary<string, string>()
        {
            { "id", "12" }
        };
        var requestUri = QueryHelpers.AddQueryString("/api/test", queryString);
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
        httpRequestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);
        httpRequestMessage.Headers.Add("Authorization", "Bearer 703438f3-16ad-4ba5-b923-8f72cd0f2db9");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        Assert.That(curlResult, Is.Not.Null);
        Assert.That(curlResult, Is.Not.Empty);
        Assert.That(curlResult, Does.StartWith("curl"));
        Assert.That(curlResult?.Trim(),
            Is.EqualTo(@"curl http://localhost:1213/v1/api/test?id=12 -H 'Authorization: Bearer 703438f3-16ad-4ba5-b923-8f72cd0f2db9' -H 'Content-Type: application/json; charset=utf-8'"));
    }

    [Theory]
    public void Success_GenerateCurlInString_When_RequestUri_Is_Null_For_GetMethod()
    {
        // Arrange
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, "");
        httpRequestMessage.Content = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json);
        httpRequestMessage.Headers.Add("Authorization", "Bearer 703438f3-16ad-4ba5-b923-8f72cd0f2db9");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        Assert.That(curlResult, Is.Not.Null);
        Assert.That(curlResult, Is.Not.Empty);
        Assert.That(curlResult, Does.StartWith("curl"));
        Assert.That(curlResult?.Trim(),
            Is.EqualTo(@"curl http://localhost:1213/v1 -H 'Authorization: Bearer 703438f3-16ad-4ba5-b923-8f72cd0f2db9' -H 'Content-Type: application/json; charset=utf-8'"));
    }

    [Theory]
    public void Success_GenerateCurlInString_Without_HttpRequestMessage_For_GetMethod()
    {
        // Arrange
        var requestUri = "/api/test";
        HttpRequestHeaders httpRequestHeaders = new HttpRequestMessage().Headers;
        httpRequestHeaders.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1");

        // Act
        string curlResult = httpClient.GenerateCurlInString(
            HttpMethod.Get, requestUri, httpRequestHeaders);

        // Assert
        Assert.That(curlResult, Is.Not.Null);
        Assert.That(curlResult, Is.Not.Empty);
        Assert.That(curlResult, Does.StartWith("curl"));
        Assert.That(curlResult?.Trim(),
            Is.EqualTo(
                @"curl http://localhost:1213/v1/api/test -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61'"));
    }

    [Theory]
    public void Success_GenerateCurlInString_Without_HttpRequestMessage_And_RequestUri_Is_Null_For_GetMethod()
    {
        // Arrange
        HttpRequestHeaders httpRequestHeaders = new HttpRequestMessage().Headers;
        httpRequestHeaders.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1");

        // Act
        string curlResult = httpClient.GenerateCurlInString(
            httpMethod: HttpMethod.Get, requestHeaders: httpRequestHeaders);

        // Assert
        Assert.That(curlResult, Is.Not.Null);
        Assert.That(curlResult, Is.Not.Empty);
        Assert.That(curlResult, Does.StartWith("curl"));
        Assert.That(curlResult?.Trim(),
            Is.EqualTo(
                @"curl http://localhost:1213/v1 -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61'"));
    }

    [Theory]
    public void Success_GenerateCurlInString_Without_HttpRequestMessage_And_HttpRequestHeader_Is_null_For_GetMethod()
    {
        // Arrange
        var requestUri = "/api/test";

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1");

        // Act
        string curlResult = httpClient.GenerateCurlInString(
            HttpMethod.Get, requestUri);

        // Assert
        Assert.That(curlResult, Is.Not.Null);
        Assert.That(curlResult, Is.Not.Empty);
        Assert.That(curlResult, Does.StartWith("curl"));
        Assert.That(curlResult?.Trim(),
            Is.EqualTo(
                @"curl http://localhost:1213/v1/api/test"));
    }

    #endregion
}