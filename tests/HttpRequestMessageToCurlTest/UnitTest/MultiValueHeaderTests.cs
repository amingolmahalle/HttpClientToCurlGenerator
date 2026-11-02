using System.Net.Http.Headers;
using FluentAssertions;
using HttpClientToCurl.Builder;
using Xunit;

namespace HttpRequestMessageToCurlTest.UnitTest;

public class MultiValueHeaderTests
{
    private static readonly Uri BaseAddress = new("http://localhost:1213/v1/");

    [Fact]
    public void Accept_Values_Are_Joined_With_Comma()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/test");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));

        var script = Generator.GenerateCurl(request, BaseAddress, null);

        script.Should().Contain("-H 'Accept: application/json, application/xml; q=0.9'");
    }

    [Fact]
    public void UserAgent_Values_Are_Joined_With_Space()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/test");
        request.Headers.UserAgent.Add(new ProductInfoHeaderValue("my-app", "1.0"));
        request.Headers.UserAgent.Add(new ProductInfoHeaderValue("(internal)"));

        var script = Generator.GenerateCurl(request, BaseAddress, null);

        script.Should().Contain("-H 'User-Agent: my-app/1.0 (internal)'");
    }

    [Fact]
    public void Repeated_Custom_Header_Values_Are_Joined_With_Comma()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/test");
        request.Headers.Add("X-Tags", ["a", "b"]);

        var script = Generator.GenerateCurl(request, BaseAddress, null);

        script.Should().Contain("-H 'X-Tags: a, b'");
    }

    [Fact]
    public void Cookie_Values_Are_Joined_With_Semicolon()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/test");
        request.Headers.Add("Cookie", ["a=1", "b=2"]);

        var script = Generator.GenerateCurl(request, BaseAddress, null);

        script.Should().Contain("-H 'Cookie: a=1; b=2'");
    }
}
