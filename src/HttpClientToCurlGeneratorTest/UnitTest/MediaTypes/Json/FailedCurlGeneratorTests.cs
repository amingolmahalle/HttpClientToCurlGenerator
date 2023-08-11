using FluentAssertions;
using HttpClientToCurl;
using NUnit.Framework;
using System.Net.Mime;
using System.Text;

namespace HttpClientToCurlGeneratorTest.UnitTest.MediaTypes.Json;

public class FailedCurlGeneratorTests
{
    [Theory]
    public void GenerateCurl_When_Invalid_HttpMethod()
    {
        // Arrange
        string requestBody = @"{""name"":""russel"",""requestId"":10001004,""amount"":50000}";

        var requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Trace, requestUri);
        httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json);
        httpRequestMessage.Headers.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            true);

        // Assert
        script.Should().NotBeNullOrEmpty();
        script.Should().StartWith("GenerateCurlError");
        script.Trim().Should().BeEquivalentTo($"GenerateCurlError => not supported {httpRequestMessage.Method.Method}!");
    }
}