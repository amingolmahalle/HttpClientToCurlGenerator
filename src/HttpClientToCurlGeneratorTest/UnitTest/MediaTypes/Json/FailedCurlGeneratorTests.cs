using System.Net.Mime;
using System.Text;
using HttpClientToCurl;
using NUnit.Framework;

namespace HttpClientToCurlGeneratorTest.UnitTest.MediaTypes.Json;

public class FailedCurlGeneratorTests
{
    #region :: GenerateCurl_For_PatchMethod ::

    [Theory]
    public void GenerateCurl_Invalid_HttpMethod()
    {
        // Arrange
        string requestBody = @"{ ""name"" : ""russel"",""requestId"" : 10001004,""amount"":50000 }";

        var requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Trace, requestUri);
        httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json);
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
        Assert.That(script, Does.StartWith("GenerateCurlError"));
        Assert.That(script?.Trim(), Is.EqualTo($"GenerateCurlError => invalid HttpMethod: {httpRequestMessage.Method.Method}!."));
    }

    [Theory]
    public void GenerateCurl_Invalid_JsonBody()
    {
        // Arrange
        string requestBody = @"""name"" : ""steven"",""requestId"" : 10001005,""amount"":60000";

        var requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
        httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json);
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
        Assert.That(script, Does.StartWith("GenerateCurlError"));
        Assert.That(script?.Trim(), Is.EqualTo("GenerateCurlError => exception in parsing body application/json!."));
    }

    [Theory]
    public void GenerateCurl_Invalid_BaseUrl()
    {
        // Arrange
        string requestBody = @"{ ""name"" : ""nancy"",""requestId"" : 10001006,""amount"":70000 }";

        var requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
        httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json);
        httpRequestMessage.Headers.Add("Authorization", "4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = null;

        // Act
        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            requestUri,
            true);

        // Assert
        Assert.That(script, Is.Not.Null);
        Assert.That(script, Is.Not.Empty);
        Assert.That(script, Does.StartWith("GenerateCurlError"));
        Assert.That(script?.Trim(), Is.EqualTo("GenerateCurlError => baseUrl argument is null or empty!."));
    }

    #endregion
}