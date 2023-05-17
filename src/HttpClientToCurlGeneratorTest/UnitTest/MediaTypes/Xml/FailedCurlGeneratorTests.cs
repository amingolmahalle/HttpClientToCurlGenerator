using System.Net.Mime;
using System.Text;
using HttpClientToCurl;
using NUnit.Framework;

namespace HttpClientToCurlGeneratorTest.UnitTest.MediaTypes.Xml;

public class FailedCurlGeneratorTests
{
    [Theory]
    public void GenerateCurl_When_Invalid_Xml()
    {
        // Arrange
        string requestBody = @"<xml version = ""1.0"" encoding = ""UTF-8""?>
            <Order>
            <Id>12</Id>
            <name>Jason</name>
            <requestId>10001024</requestId>
            <amount>240000</amount>
            </Order>";

        var requestUri = "/api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
        httpRequestMessage.Content = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Text.Xml);
        httpRequestMessage.Headers.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("http://localhost:1213/v1");

        // Act
        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            true);

        // Assert
        Assert.That(!string.IsNullOrWhiteSpace(script?.Trim()), Is.True);
        Assert.That(script, Does.StartWith("GenerateCurlError"));
        Assert.That(script?.Trim(), Is.EqualTo(@"GenerateCurlError => exception in parsing request body text/xml!
request body:
<xml version = ""1.0"" encoding = ""UTF-8""?>
            <Order>
            <Id>12</Id>
            <name>Jason</name>
            <requestId>10001024</requestId>
            <amount>240000</amount>
            </Order>"));
    }
}