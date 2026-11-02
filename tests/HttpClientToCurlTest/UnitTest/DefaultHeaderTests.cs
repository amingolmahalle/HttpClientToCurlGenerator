using System.Net.Http.Headers;
using FluentAssertions;
using HttpClientToCurl.Builder;
using HttpClientToCurl.Config;
using Xunit;

namespace HttpClientToCurlTest.UnitTest;

/// <summary>Behaviour specific to the <see cref="HttpClient"/> overload: headers set on DefaultRequestHeaders.</summary>
public class DefaultHeaderTests
{
    private static HttpClient CreateClient()
        => new() { BaseAddress = new Uri("http://localhost:1213/v1/") };

    [Fact]
    public void Apostrophe_In_Default_Header_Is_Escaped()
    {
        using var client = CreateClient();
        client.DefaultRequestHeaders.Add("X-Name", "O'Brien");

        var script = Generator.GenerateCurl(client, new HttpRequestMessage(HttpMethod.Get, "api/test"), null);

        script.Should().Contain(@"-H 'X-Name: O'\''Brien'");
    }

    [Fact]
    public void Redacted_Default_Header_Is_Redacted_Regardless_Of_Casing()
    {
        using var client = CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer secret");

        var script = Generator.GenerateCurl(client, new HttpRequestMessage(HttpMethod.Get, "api/test"), new BaseConfig { RedactedHeaders = ["authorization"] });

        script.Should().Contain("-H 'Authorization: [REDACTED]'").And.NotContain("secret");
    }

    [Fact]
    public void Default_Accept_Values_Are_Joined_With_Comma()
    {
        using var client = CreateClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml", 0.9));

        var script = Generator.GenerateCurl(client, new HttpRequestMessage(HttpMethod.Get, "api/test"), null);

        script.Should().Contain("-H 'Accept: application/json, application/xml; q=0.9'");
    }
}
