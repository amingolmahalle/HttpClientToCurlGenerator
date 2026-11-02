using System.Net.Mime;
using System.Text;
using FluentAssertions;
using HttpClientToCurl.Config;
using Xunit;

namespace SharedCurlTests;

public abstract class MethodHandlingTestsBase
{
    private static readonly Uri BaseAddress = new("http://localhost:1213/v1/");
    private const string Json = @"{""name"":""russel"",""amount"":50000}";

    protected abstract string Generate(HttpRequestMessage request, Uri? baseAddress, BaseConfig? config);

    private string Curl(HttpRequestMessage request) => Generate(request, BaseAddress, null).Trim();

    private static HttpRequestMessage Request(HttpMethod method, bool withBody = false)
    {
        var request = new HttpRequestMessage(method, "api/test");
        request.Headers.Add("Authorization", "Bearer token");
        if (withBody)
        {
            request.Content = new StringContent(Json, Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        return request;
    }

    [Fact]
    public void Head_Uses_Curl_Head_Flag_Instead_Of_X_Head()
        => Curl(Request(HttpMethod.Head))
            .Should().Be("curl --head 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer token'");

    [Fact]
    public void Options_Is_Supported()
        => Curl(Request(HttpMethod.Options))
            .Should().Be("curl -X OPTIONS 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer token'");

    [Fact]
    public void Trace_Is_Supported_And_No_Longer_Throws()
        => Curl(Request(HttpMethod.Trace))
            .Should().Be("curl -X TRACE 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer token'");

    [Fact]
    public void Unknown_Method_With_A_Body_Includes_The_Body()
        => Curl(Request(new HttpMethod("PROPFIND"), withBody: true))
            .Should().Be(@"curl -X PROPFIND 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer token' -H 'Content-Type: application/json; charset=utf-8' -d '{""name"":""russel"",""amount"":50000}'");

    [Fact]
    public void Unknown_Method_Without_A_Body_Does_Not_Emit_An_Empty_Data_Argument()
        => Curl(Request(new HttpMethod("PURGE")))
            .Should().Be("curl -X PURGE 'http://localhost:1213/v1/api/test' -H 'Authorization: Bearer token'");

    [Fact]
    public void Method_Containing_Shell_Metacharacters_Is_Quoted()
        => Curl(Request(new HttpMethod("A`id`")))
            .Should().StartWith("curl -X 'A`id`' ");

    [Fact]
    public void Known_Methods_Keep_Their_Existing_Output()
    {
        Curl(Request(HttpMethod.Get)).Should().StartWith("curl 'http://");
        Curl(Request(HttpMethod.Delete)).Should().StartWith("curl -X DELETE 'http://");
        Curl(Request(HttpMethod.Post, withBody: true)).Should().StartWith("curl -X POST 'http://").And.Contain("-d '");
        Curl(Request(HttpMethod.Put, withBody: true)).Should().StartWith("curl -X PUT 'http://").And.Contain("-d '");
        Curl(Request(HttpMethod.Patch, withBody: true)).Should().StartWith("curl -X PATCH 'http://").And.Contain("-d '");
    }

    [Fact]
    public void Head_With_Content_Does_Not_Emit_A_Body_Because_Curl_Rejects_Head_With_Data()
    {
        var curl = Curl(Request(HttpMethod.Head, withBody: true));

        curl.Should().StartWith("curl --head ");
        curl.Should().NotContain(" -d ").And.NotContain("--data");
    }
}
