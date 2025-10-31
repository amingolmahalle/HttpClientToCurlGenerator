using System.Net.Mime;
using System.Text;
using FluentAssertions;
using HttpClientToCurl.Extensions;
using HttpClientToCurl.Utility;
using Microsoft.AspNetCore.WebUtilities;
using Xunit;

namespace HttpRequestMessageToCurlTest.FunctionalTest;

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
        var baseAddress = new Uri("http://localhost:1213/v1/");
        // Act
        string curlResult = httpRequestMessage.GenerateCurlInString(baseAddress);

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

        var baseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpRequestMessage.GenerateCurlInString(baseAddress);

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
        var baseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpRequestMessage.GenerateCurlInString(baseAddress);

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

        var baseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpRequestMessage.GenerateCurlInString(baseAddress);

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

        var baseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpRequestMessage.GenerateCurlInString(baseAddress);

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
        var baseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpRequestMessage.GenerateCurlInString(baseAddress);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl -X POST");
        curlResult.Trim().Should().BeEquivalentTo(@"curl -X POST 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -d ''");
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

        var baseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpRequestMessage.GenerateCurlInString(baseAddress);

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

        var baseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpRequestMessage.GenerateCurlInString(baseAddress);

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

        var baseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpRequestMessage.GenerateCurlInString(baseAddress);

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

        var baseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpRequestMessage.GenerateCurlInString(baseAddress);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl");
        curlResult.Trim().Should()
            .BeEquivalentTo(@"curl 'http://localhost:1213/v1/' -H 'Authorization: Bearer 703438f3-16ad-4ba5-b923-8f72cd0f2db9' -H 'Content-Type: application/json; charset=utf-8'");
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

        var baseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string curlResult = httpRequestMessage.GenerateCurlInString(baseAddress);

        // Assert
        curlResult.Should().NotBeNullOrEmpty();
        curlResult.Should().StartWith("curl");
        curlResult.Trim().Should()
            .BeEquivalentTo(
                @"curl 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer 703438f3-16ad-4ba5-b923-8f72cd0f2db9' -H 'Cookie: _ga=GA1.1.41226618.1701506283; mywebsite-sp=cbf42587-7ec5-4179-aac5-cbc9ae6fbf05; sp_ses.13cb=*' -H 'Content-Type: application/json; charset=utf-8'");
    }

    #endregion
}
