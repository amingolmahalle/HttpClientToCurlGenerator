using System.Net.Mime;
using System.Text;
using FluentAssertions;
using HttpClientToCurl.Builder;
using Xunit;

namespace HttpRequestMessageToCurlTest.UnitTest.MediaTypes.Xml;

public class SuccessCurlGeneratorTests
{
    [Fact]
    public void Get_Curl_Script_For_PostMethod()
    {
        // Arrange
        string requestBody = @"<?xml version = ""1.0"" encoding = ""UTF-8""?>
            <Order>
            <Id>12</Id>
            <name>Jason</name>
            <requestId>10001024</requestId>
            <amount>240000</amount>
            </Order>";

        var requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Text.Xml) };
        httpRequestMessage.Headers.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        var baseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string script = Generator.GenerateCurl(
            httpRequestMessage,
            baseAddress,
            null);

        // Assert
        script.Should().NotBeNullOrEmpty();
        script.Should().StartWith("curl -X POST");
        script.Trim().Should().BeEquivalentTo(
            @"curl -X POST 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: text/xml; charset=utf-8' -d '<?xml version = ""1.0"" encoding = ""UTF-8""?>
            <Order>
            <Id>12</Id>
            <name>Jason</name>
            <requestId>10001024</requestId>
            <amount>240000</amount>
            </Order>'");
    }

    [Fact]
    public void Get_Error_Message_When_Input_XML_Is_Invalid_For_PostMethod()
    {
        // Arrange
        string requestBody = @"<xml version = ""1.0"" encoding = ""UTF-8""?>
            <Order>
            <Id>12</Id>
            <name>Jason</name>
            <requestId>10001024</requestId>
            <amount>240000</amount>
            </Order>";

        var requestUri = "api/test";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = new StringContent(requestBody, Encoding.UTF8, MediaTypeNames.Text.Xml) };
        httpRequestMessage.Headers.Add("Authorization", "Bearer 4797c126-3f8a-454a-aff1-96c0220dae61");

        var baseAddress = new Uri("http://localhost:1213/v1/");

        // Act
        string script = Generator.GenerateCurl(
            httpRequestMessage,
            baseAddress,
            null);

        // Assert
        script.Should().NotBeNull();
        script.Should().NotBeEmpty();
        script.Should().StartWith("curl -X POST");
        script.Trim().Should().BeEquivalentTo(
            @"curl -X POST 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer 4797c126-3f8a-454a-aff1-96c0220dae61' -H 'Content-Type: text/xml; charset=utf-8' -d '<xml version = ""1.0"" encoding = ""UTF-8""?>
            <Order>
            <Id>12</Id>
            <name>Jason</name>
            <requestId>10001024</requestId>
            <amount>240000</amount>
            </Order>'");
    }
}
