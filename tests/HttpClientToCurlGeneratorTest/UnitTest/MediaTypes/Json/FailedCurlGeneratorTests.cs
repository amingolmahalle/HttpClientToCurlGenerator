using FluentAssertions;
using HttpClientToCurl;
using System.Net.Mime;
using System.Text;
using HttpClientToCurl.Builder;
using Xunit;

namespace HttpClientToCurlGeneratorTest.UnitTest.MediaTypes.Json;

public class FailedCurlGeneratorTests
{
    [Fact]
    public void GenerateCurl_When_HttpMethod_Is_Invalid()
    {
        // Arrange
        string requestBody = /*lang=json,strict*/ @"{""name"":""russel"",""requestId"":10001004,""amount"":50000}";

        var requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Trace, requestUri)
        {
            Content = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Application.Json)
        };
        httpRequestMessage.Headers.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            null);

        // Assert
        script.Should().NotBeNullOrEmpty();
        script.Should().StartWith("GenerateCurlError => not supported");
        script.Trim().Should().BeEquivalentTo($"GenerateCurlError => not supported {httpRequestMessage.Method.Method}!");
    }
}
