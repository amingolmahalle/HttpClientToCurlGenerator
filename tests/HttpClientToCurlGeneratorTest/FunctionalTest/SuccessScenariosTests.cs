using FluentAssertions;
using HttpClientToCurl;
using HttpClientToCurl.Utility;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using Xunit;

namespace HttpClientToCurlGeneratorTest.FunctionalTest;

public class SuccessScenariosTests
{
    #region :: GenerateCurlInString For Post Method ::

    [Fact]
    public void Success_GenerateCurlInString_For_PostMethod()
    {
        // Arrange
        string requestBody = /*lang=json,strict*/ @"{""name"":""sara"",""requestId"":10001001,""amount"":20000}";

        var requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json) };
        httpRequestMessage.Headers.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl -X POST");
        curlResult.Trim().Should()
            .BeEquivalentTo(
                @"curl -X POST 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d '{""name"":""sara"",""requestId"":10001001,""amount"":20000}'");
    }

    [Fact]
    public void Success_GenerateCurlInString_With_RequestUri_TypeOf_Uri_For_PostMethod()
    {
        // Arrange
        string requestBody = /*lang=json,strict*/ @"{""name"":""sara"",""requestId"":10001001,""amount"":20000}";

        var requestUri = Helpers.CreateUri("api/test");
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json) };
        httpRequestMessage.Headers.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl -X POST");
        curlResult.Trim().Should()
            .BeEquivalentTo(
                @"curl -X POST 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d '{""name"":""sara"",""requestId"":10001001,""amount"":20000}'");
    }

    [Fact]
    public void GenerateCurl_When_Set_RequestUri_Inside_HttpRequestMessage_For_PostMethod()
    {
        // Arrange
        string requestBody = /*lang=json,strict*/ @"{""name"":""sara"",""requestId"":10001001,""amount"":20000}";

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "api/test") { Content = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json) };
        httpRequestMessage.Headers.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl -X POST");
        curlResult.Trim().Should()
            .BeEquivalentTo(
                @"curl -X POST 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d '{""name"":""sara"",""requestId"":10001001,""amount"":20000}'");
    }

    [Fact]
    public void Success_GenerateCurlInString_When_RequestUri_Is_Null_For_PostMethod()
    {
        // Arrange
        string requestBody = /*lang=json,strict*/ @"{""name"":""sara"",""requestId"":10001001,""amount"":20000}";

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, string.Empty) { Content = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json) };
        httpRequestMessage.Headers.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl -X POST");
        curlResult.Trim().Should()
            .BeEquivalentTo(
                @"curl -X POST 'http://localhost:1213/v1/' -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d '{""name"":""sara"",""requestId"":10001001,""amount"":20000}'");
    }

    [Fact]
    public void Success_GenerateCurlInString_When_RequestBody_Is_Null_For_PostMethod()
    {
        // Arrange
        var requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json) };
        httpRequestMessage.Headers.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl -X POST");
        curlResult.Trim().Should()
            .BeEquivalentTo(
                @"curl -X POST 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d ''");
    }

    [Fact]
    public void Success_GenerateCurlInString_When_HttpContent_Is_Null_For_PostMethod()
    {
        // Arrange
        var requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
        httpRequestMessage.Headers.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl -X POST");
        curlResult.Trim().Should().BeEquivalentTo(@"curl -X POST 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -d ''");
    }

    [Fact]
    public void Success_GenerateCurlInString_Without_HttpRequestMessage_For_PostMethod()
    {
        // Arrange
        var requestObject = new { name = "sara", requestId = 10001001, amount = 20000, };

        JsonContent jsonContent = JsonContent.Create(requestObject);

        var requestUri = "api/test";
        HttpRequestHeaders httpRequestHeaders = new HttpRequestMessage().Headers;
        httpRequestHeaders.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(
            HttpMethod.Post, requestUri, httpRequestHeaders, jsonContent);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl -X POST");
        curlResult.Trim().Should()
            .BeEquivalentTo(
                @"curl -X POST 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d '{""name"":""sara"",""requestId"":10001001,""amount"":20000}'");
    }

    [Fact]
    public void Success_GenerateCurlInString_Without_HttpRequestMessage_With_RequestUri_Typeof_Uri_For_PostMethod()
    {
        // Arrange
        var requestObject = new { name = "sara", requestId = 10001001, amount = 20000, };

        JsonContent jsonContent = JsonContent.Create(requestObject);

        var requestUri = Helpers.CreateUri("api/test");
        HttpRequestHeaders httpRequestHeaders = new HttpRequestMessage().Headers;
        httpRequestHeaders.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(
            HttpMethod.Post, requestUri, httpRequestHeaders, jsonContent);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl -X POST");
        curlResult.Trim().Should()
            .BeEquivalentTo(
                @"curl -X POST 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d '{""name"":""sara"",""requestId"":10001001,""amount"":20000}'");
    }

    [Fact]
    public void Success_GenerateCurlInString_Without_HttpRequestMessage_And_Body_Is_Null_For_PostMethod()
    {
        // Arrange
        var requestUri = "api/test";
        HttpRequestHeaders httpRequestHeaders = new HttpRequestMessage().Headers;
        httpRequestHeaders.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(
            HttpMethod.Post, requestUri, httpRequestHeaders);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl -X POST");
        curlResult.Trim().Should().BeEquivalentTo(@"curl -X POST 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -d ''");
    }

    [Fact]
    public void Success_GenerateCurlInString_Without_HttpRequestMessage_And_RequestUri_Is_Null_For_PostMethod()
    {
        // Arrange
        var requestObject = new { name = "sara", requestId = 10001001, amount = 20000, };

        JsonContent jsonContent = JsonContent.Create(requestObject);

        HttpRequestHeaders httpRequestHeaders = new HttpRequestMessage().Headers;
        httpRequestHeaders.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(
            httpMethod: HttpMethod.Post, httpRequestHeaders: httpRequestHeaders, httpContent: jsonContent, requestUri: string.Empty);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl -X POST");
        curlResult.Trim().Should()
            .BeEquivalentTo(
                @"curl -X POST 'http://localhost:1213/v1/' -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: application/json; charset=utf-8' -d '{""name"":""sara"",""requestId"":10001001,""amount"":20000}'");
    }

    [Fact]
    public void Success_GenerateCurlInString_Without_HttpRequestMessage_And_HttpRequestHeader_Is_null_For_PostMethod()
    {
        // Arrange
        var requestObject = new { name = "sara", requestId = 10001001, amount = 20000, };

        JsonContent jsonContent = JsonContent.Create(requestObject);

        var requestUri = "api/test";

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(
            httpMethod: HttpMethod.Post, requestUri: requestUri, httpContent: jsonContent);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl -X POST");
        curlResult.Trim().Should()
            .BeEquivalentTo(
                @"curl -X POST 'http://localhost:1213/v1/api/test' -H 'Content-Type: application/json; charset=utf-8' -d '{""name"":""sara"",""requestId"":10001001,""amount"":20000}'");
    }

    [Fact]
    public void Success_GenerateCurlInString_With_Multiple_Value_For_A_Header_PostMethod()
    {
        // Arrange
        string requestBody = /*lang=json,strict*/ @"{""name"":""sara"",""requestId"":10001001,""amount"":20000}";

        var requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json) };
        httpRequestMessage.Headers.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        List<string?> headerValues = ["_ga=GA1.1.41226618.1701506283", "mywebsite-sp=cbf42587-7ec5-4179-aac5-cbc9ae6fbf05", "sp_ses.13cb=*"];
        httpRequestMessage.Headers.Add("cookie", headerValues);

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl -X POST");
        curlResult.Trim().Should()
            .BeEquivalentTo(
                @"curl -X POST 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Cookie: _ga=GA1.1.41226618.1701506283; mywebsite-sp=cbf42587-7ec5-4179-aac5-cbc9ae6fbf05; sp_ses.13cb=*' -H 'Content-Type: application/json; charset=utf-8' -d '{""name"":""sara"",""requestId"":10001001,""amount"":20000}'");
    }

    #endregion

    #region :: GenerateCurlInString For Get Method ::

    [Fact]
    public void Success_GenerateCurlInString_For_GetMethod()
    {
        // Arrange
        var requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri) { Content = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json) };
        httpRequestMessage.Headers.Add("Authorization", "Bearer 703438f3-16ad-4ba5-b923-8f72cd0f2db9");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl");
        curlResult.Trim().Should()
            .BeEquivalentTo(
                @"curl 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer 703438f3-16ad-4ba5-b923-8f72cd0f2db9' -H 'Content-Type: application/json; charset=utf-8'");
    }

    [Fact]
    public void Success_GenerateCurlInString_With_QueryString_For_GetMethod()
    {
        // Arrange
        var queryString = new Dictionary<string, string> { { "id", "12" } };
        var requestUri = QueryHelpers.AddQueryString("api/test", queryString!);
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri) { Content = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json) };
        httpRequestMessage.Headers.Add("Authorization", "Bearer 703438f3-16ad-4ba5-b923-8f72cd0f2db9");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl");
        curlResult.Trim().Should()
            .BeEquivalentTo(
                @"curl 'http://localhost:1213/v1/api/test?id=12' -H 'Authorization: Bearer 703438f3-16ad-4ba5-b923-8f72cd0f2db9' -H 'Content-Type: application/json; charset=utf-8'");
    }

    [Fact]
    public void Success_GenerateCurlInString_When_RequestUri_Is_Null_For_GetMethod()
    {
        // Arrange
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, string.Empty) { Content = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json) };
        httpRequestMessage.Headers.Add("Authorization", "Bearer 703438f3-16ad-4ba5-b923-8f72cd0f2db9");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl");
        curlResult.Trim().Should()
            .BeEquivalentTo(@"curl 'http://localhost:1213/v1/' -H 'Authorization: Bearer 703438f3-16ad-4ba5-b923-8f72cd0f2db9' -H 'Content-Type: application/json; charset=utf-8'");
    }

    [Fact]
    public void Success_GenerateCurlInString_Without_HttpRequestMessage_For_GetMethod()
    {
        // Arrange
        var requestUri = "api/test";
        HttpRequestHeaders httpRequestHeaders = new HttpRequestMessage().Headers;
        httpRequestHeaders.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(
            HttpMethod.Get, requestUri, httpRequestHeaders);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl");
        curlResult.Trim().Should().BeEquivalentTo(@"curl 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61'");
    }

    [Fact]
    public void Success_GenerateCurlInString_Without_HttpRequestMessage_And_RequestUri_Is_Null_For_GetMethod()
    {
        // Arrange
        HttpRequestHeaders httpRequestHeaders = new HttpRequestMessage().Headers;
        httpRequestHeaders.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(
            httpMethod: HttpMethod.Get, httpRequestHeaders: httpRequestHeaders);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl");
        curlResult.Trim().Should().BeEquivalentTo(@"curl 'http://localhost:1213/v1/' -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61'");
    }

    [Fact]
    public void Success_GenerateCurlInString_Without_HttpRequestMessage_And_HttpRequestHeader_Is_null_For_GetMethod()
    {
        // Arrange
        var requestUri = "api/test";

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(
            HttpMethod.Get, requestUri);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl");
        curlResult.Trim().Should().BeEquivalentTo(@"curl 'http://localhost:1213/v1/api/test'");
    }

    [Fact]
    public void Success_GenerateCurlInString_With_Multiple_Value_For_A_Header_GetMethod()
    {
        // Arrange
        var requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri) { Content = new StringContent(string.Empty, Encoding.UTF8, MediaTypeNames.Application.Json) };
        httpRequestMessage.Headers.Add("Authorization", "Bearer 703438f3-16ad-4ba5-b923-8f72cd0f2db9");

        List<string?> headerValues = ["_ga=GA1.1.41226618.1701506283", "mywebsite-sp=cbf42587-7ec5-4179-aac5-cbc9ae6fbf05", "sp_ses.13cb=*"];
        httpRequestMessage.Headers.Add("cookie", headerValues);

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpClient.GenerateCurlInString(httpRequestMessage);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl");
        curlResult.Trim().Should()
            .BeEquivalentTo(
                @"curl 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer 703438f3-16ad-4ba5-b923-8f72cd0f2db9' -H 'Cookie: _ga=GA1.1.41226618.1701506283; mywebsite-sp=cbf42587-7ec5-4179-aac5-cbc9ae6fbf05; sp_ses.13cb=*' -H 'Content-Type: application/json; charset=utf-8'");
    }

    #endregion
}
