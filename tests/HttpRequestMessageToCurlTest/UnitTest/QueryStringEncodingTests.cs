using FluentAssertions;
using HttpClientToCurl.Builder;
using Xunit;

namespace HttpRequestMessageToCurlTest.UnitTest;

public class QueryStringEncodingTests
{
    private static readonly Uri BaseAddress = new("http://localhost:1213/v1/");

    private static string Curl(string relativeUri)
        => Generator.GenerateCurl(new HttpRequestMessage(HttpMethod.Get, relativeUri), BaseAddress, null).Trim();

    [Fact]
    public void Single_Parameter_Value_Is_Encoded()
        => Curl("api/test?q=a b").Should().Be("curl 'http://localhost:1213/v1/api/test?q=a%20b'");

    [Fact]
    public void Parameter_Without_Equals_Sign_Is_Kept()
        => Curl("api/test?a=1&flag&b=2").Should().Be("curl 'http://localhost:1213/v1/api/test?a=1&flag&b=2'");

    [Fact]
    public void Empty_Value_Is_Kept()
        => Curl("api/test?a=1&b=").Should().Be("curl 'http://localhost:1213/v1/api/test?a=1&b='");

    [Fact]
    public void Value_Containing_Equals_Sign_Is_Not_Truncated()
        => Curl("api/test?token=YWJj==&x=1").Should().Be("curl 'http://localhost:1213/v1/api/test?token=YWJj%3D%3D&x=1'");

    [Fact]
    public void Already_Encoded_Value_Is_Not_Encoded_Twice_With_Multiple_Parameters()
        => Curl("api/test?name=a%20b&x=1").Should().Be("curl 'http://localhost:1213/v1/api/test?name=a%20b&x=1'");

    [Fact]
    public void Already_Encoded_Value_Is_Not_Encoded_Twice_With_Single_Parameter()
        => Curl("api/test?name=a%20b").Should().Be("curl 'http://localhost:1213/v1/api/test?name=a%20b'");

    [Fact]
    public void Encoded_Ampersand_Inside_A_Value_Stays_Inside_The_Value()
        => Curl("api/test?name=a%26b&x=1").Should().Be("curl 'http://localhost:1213/v1/api/test?name=a%26b&x=1'");

    [Fact]
    public void Query_Without_Parameters_Is_Left_Alone()
        => Curl("api/test").Should().Be("curl 'http://localhost:1213/v1/api/test'");

    [Fact]
    public void Literal_Plus_In_Single_Parameter_Value_Is_Kept_As_Plus()
        => Curl("api/test?q=a+b").Should().Be("curl 'http://localhost:1213/v1/api/test?q=a+b'");

    [Fact]
    public void Literal_Plus_In_Multi_Parameter_Value_Is_Kept_As_Plus()
        => Curl("api/test?q=a+b&x=1").Should().Be("curl 'http://localhost:1213/v1/api/test?q=a+b&x=1'");

    [Fact]
    public void Encoded_Plus_Stays_Encoded()
        => Curl("api/test?q=a%2Bb&x=1").Should().Be("curl 'http://localhost:1213/v1/api/test?q=a%2Bb&x=1'");
}
