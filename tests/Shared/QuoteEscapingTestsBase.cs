using System.Text;
using FluentAssertions;
using HttpClientToCurl.Config;
using Xunit;

namespace SharedCurlTests;

public abstract class QuoteEscapingTestsBase
{
    protected static readonly Uri BaseAddress = new("http://localhost:1213/v1/");

    protected abstract string Generate(HttpRequestMessage request, Uri? baseAddress, BaseConfig? config);

    private string Curl(HttpRequestMessage request) => Generate(request, BaseAddress, null);

    [Fact]
    public void Apostrophe_In_Json_Body_Is_Escaped()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/test")
        {
            Content = new StringContent(@"{""name"":""O'Brien""}", Encoding.UTF8, "application/json")
        };

        Curl(request).Should().Contain(@"-d '{""name"":""O'\''Brien""}'");
    }

    [Fact]
    public void Apostrophe_In_FormUrlEncoded_Body_Is_Escaped()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/test")
        {
            Content = new FormUrlEncodedContent([new KeyValuePair<string, string>("name", "O'Brien")])
        };

        Curl(request).Should().Contain(@"-d 'name=O'\''Brien'");
    }

    [Fact]
    public void Apostrophe_In_Request_Header_Is_Escaped()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/test");
        request.Headers.Add("X-Name", "a'; echo pwned; echo 'b");

        Curl(request).Should().Contain(@"-H 'X-Name: a'\''; echo pwned; echo '\''b'");
    }

    [Fact]
    public void Apostrophe_In_Content_Header_Is_Escaped()
    {
        var content = new StringContent("{}", Encoding.UTF8, "application/json");
        content.Headers.Add("X-Content-Note", "O'Brien");
        var request = new HttpRequestMessage(HttpMethod.Post, "api/test") { Content = content };

        Curl(request).Should().Contain(@"-H 'X-Content-Note: O'\''Brien'");
    }

    [Fact]
    public void Apostrophe_In_Url_Produces_Exact_Escaped_Output()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/it's");

        Curl(request).Trim().Should().Be(@"curl 'http://localhost:1213/v1/api/it'\''s'");
    }

    [Fact]
    public void Apostrophe_In_Single_Parameter_Query_Is_Percent_Encoded()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/test?q=it's");

        Curl(request).Trim().Should().Be("curl 'http://localhost:1213/v1/api/test?q=it%27s'");
    }

    [Fact]
    public void Apostrophe_In_Multi_Parameter_Query_Never_Appears_Unescaped()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/test?a=1&q=it's");

        Curl(request).Should().NotContain("it's");
    }
}
