using System.Net.Http.Headers;
using FluentAssertions;
using HttpClientToCurl.Builder;
using Xunit;

namespace HttpClientToCurlTest.UnitTest;

public class MultiValueHeaderTests
{
    [Fact]
    public void Default_Accept_Values_Are_Joined_With_Comma()
    {
        using var client = new HttpClient { BaseAddress = new Uri("http://localhost:1213/v1/") };
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));

        var script = Generator.GenerateCurl(client, new HttpRequestMessage(HttpMethod.Get, "api/test"), null);

        script.Should().Contain("-H 'Accept: application/json, application/xml; q=0.9'");
    }
}
