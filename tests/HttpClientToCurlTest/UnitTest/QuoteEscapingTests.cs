using System.Text;
using FluentAssertions;
using HttpClientToCurl.Builder;
using HttpClientToCurl.Config;
using Xunit;

namespace HttpClientToCurlTest.UnitTest;

public class QuoteEscapingTests
{
    private static HttpClient CreateClient()
        => new() { BaseAddress = new Uri("http://localhost:1213/v1/") };

    [Fact]
    public void Apostrophe_In_Json_Body_Is_Escaped()
    {
        using var client = CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "api/test")
        {
            Content = new StringContent(@"{""name"":""O'Brien""}", Encoding.UTF8, "application/json")
        };

        var script = Generator.GenerateCurl(client, request, null);

        script.Should().Contain(@"-d '{""name"":""O'\''Brien""}'");
    }

    [Fact]
    public void Apostrophe_In_FormUrlEncoded_Body_Is_Escaped()
    {
        using var client = CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "api/test")
        {
            Content = new FormUrlEncodedContent([new KeyValuePair<string, string>("name", "O'Brien")])
        };

        var script = Generator.GenerateCurl(client, request, null);

        script.Should().Contain(@"-d 'name=O'\''Brien'");
    }

    [Fact]
    public void Apostrophe_In_Default_Header_Is_Escaped()
    {
        using var client = CreateClient();
        client.DefaultRequestHeaders.Add("X-Name", "O'Brien");
        var request = new HttpRequestMessage(HttpMethod.Get, "api/test");

        var script = Generator.GenerateCurl(client, request, null);

        script.Should().Contain(@"-H 'X-Name: O'\''Brien'");
    }

    [Fact]
    public void Apostrophe_In_Request_Header_Is_Escaped()
    {
        using var client = CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "api/test");
        request.Headers.Add("X-Name", "a'; echo pwned; echo 'b");

        var script = Generator.GenerateCurl(client, request, null);

        script.Should().Contain(@"-H 'X-Name: a'\''; echo pwned; echo '\''b'");
    }

    [Fact]
    public void Apostrophe_In_Url_Produces_Exact_Escaped_Output()
    {
        using var client = CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "api/it's");

        var script = Generator.GenerateCurl(client, request, null);

        script.Trim().Should().Be(@"curl 'http://localhost:1213/v1/api/it'\''s'");
    }

    [Fact]
    public void Redacted_Default_Header_Is_Redacted_Regardless_Of_Casing()
    {
        using var client = CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", "Bearer secret");
        var request = new HttpRequestMessage(HttpMethod.Get, "api/test");

        var script = Generator.GenerateCurl(client, request, new BaseConfig { RedactedHeaders = ["authorization"] });

        script.Should().Contain("-H 'Authorization: [REDACTED]'").And.NotContain("secret");
    }
}
