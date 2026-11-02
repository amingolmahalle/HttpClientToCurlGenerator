using System.Text;
using FluentAssertions;
using HttpClientToCurl.Builder;
using Xunit;

namespace HttpRequestMessageToCurlTest.UnitTest;

public class QuoteEscapingTests
{
    private static readonly Uri BaseAddress = new("http://localhost:1213/v1/");

    [Fact]
    public void Apostrophe_In_Body_Is_Escaped()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "api/test")
        {
            Content = new StringContent(@"{""name"":""O'Brien""}", Encoding.UTF8, "application/json")
        };

        var script = Generator.GenerateCurl(request, BaseAddress, null);

        script.Should().Contain(@"-d '{""name"":""O'\''Brien""}'");
    }

    [Fact]
    public void Apostrophe_In_Header_Is_Escaped()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/test");
        request.Headers.Add("X-Name", "O'Brien'; touch /tmp/pwned; echo '");

        var script = Generator.GenerateCurl(request, BaseAddress, null);

        script.Should().Contain(@"-H 'X-Name: O'\''Brien'\''; touch /tmp/pwned; echo '\'''");
    }

    [Fact]
    public void Apostrophe_In_Url_Is_Escaped()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/it's");

        var script = Generator.GenerateCurl(request, BaseAddress, null);

        script.Should().NotContain("it's");
        script.Should().Contain(@"'\''");
    }

    [Fact]
    public void Apostrophe_In_Content_Header_Is_Escaped()
    {
        var content = new StringContent("{}", Encoding.UTF8, "application/json");
        content.Headers.Add("X-Content-Note", "O'Brien");
        var request = new HttpRequestMessage(HttpMethod.Post, "api/test") { Content = content };

        var script = Generator.GenerateCurl(request, BaseAddress, null);

        script.Should().Contain(@"-H 'X-Content-Note: O'\''Brien'");
    }

    [Fact]
    public void Apostrophe_In_Single_Parameter_Query_Is_Percent_Encoded()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/test?q=it's");

        var script = Generator.GenerateCurl(request, BaseAddress, null);

        script.Trim().Should().Be("curl 'http://localhost:1213/v1/api/test?q=it%27s'");
    }

    [Fact]
    public void Apostrophe_In_Multi_Parameter_Query_Never_Appears_Unescaped()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "api/test?a=1&q=it's");

        var script = Generator.GenerateCurl(request, BaseAddress, null);

        script.Should().NotContain("it's");
    }
}
