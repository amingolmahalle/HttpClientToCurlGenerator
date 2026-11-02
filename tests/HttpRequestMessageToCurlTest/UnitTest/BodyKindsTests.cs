using System.Text;
using FluentAssertions;
using HttpClientToCurl.Builder;
using Xunit;

namespace HttpRequestMessageToCurlTest.UnitTest;

public class BodyKindsTests
{
    private static readonly Uri BaseAddress = new("http://localhost:1213/v1/");

    private static HttpRequestMessage Post(HttpContent content)
        => new(HttpMethod.Post, "api/test") { Content = content };

    [Fact]
    public void Body_Starting_With_At_Sign_Is_Sent_Raw_Not_Read_From_A_File()
    {
        var request = Post(new StringContent("@/etc/passwd", Encoding.UTF8, "text/plain"));

        var script = Generator.GenerateCurl(request, BaseAddress, null);

        script.Should().Contain("--data-raw '@/etc/passwd'");
        script.Should().NotContain(" -d '@");
    }

    [Fact]
    public void Multipart_Form_Data_Is_Emitted_As_Form_Parts()
    {
        var content = new MultipartFormDataContent("test-boundary")
        {
            { new StringContent("O'Brien"), "name" },
            { new StringContent("@not-a-file"), "note" },
            { new ByteArrayContent([1, 2, 3]) { Headers = { ContentType = new("image/png") } }, "avatar", "pic.png" },
        };

        var script = Generator.GenerateCurl(Post(content), BaseAddress, null);

        script.Should().Contain(@"--form-string 'name=O'\''Brien'");
        script.Should().Contain("--form-string 'note=@not-a-file'");
        script.Should().Contain("-F 'avatar=@pic.png;type=image/png'");
        script.Should().NotContain("boundary", "curl generates its own boundary for -F");
        script.Should().NotContain(" -d ");
    }

    [Fact]
    public void Multipart_File_Name_With_Special_Characters_Is_Quoted()
    {
        var content = new MultipartFormDataContent("test-boundary")
        {
            { new ByteArrayContent([1]) { Headers = { ContentType = new("text/plain") } }, "doc", "a;b.txt" },
        };

        var script = Generator.GenerateCurl(Post(content), BaseAddress, null);

        script.Should().Contain("-F 'doc=@\"a;b.txt\";type=text/plain'");
    }

    [Fact]
    public void Binary_Body_Is_Emitted_As_Data_Binary_Placeholder()
    {
        var content = new ByteArrayContent([0, 1, 2, 255]);
        content.Headers.ContentType = new("application/octet-stream");

        var script = Generator.GenerateCurl(Post(content), BaseAddress, null);

        script.Should().Contain("-H 'Content-Type: application/octet-stream'");
        script.Should().Contain("--data-binary '@payload.bin'");
        script.Should().NotContain(" -d ");
    }

    [Theory]
    [InlineData("application/json")]
    [InlineData("application/problem+json")]
    [InlineData("application/xml")]
    [InlineData("text/plain")]
    [InlineData("application/x-www-form-urlencoded")]
    public void Text_Like_Body_Types_Keep_Using_Data(string mediaType)
    {
        var content = new ByteArrayContent(Encoding.UTF8.GetBytes("a=1"));
        content.Headers.ContentType = new(mediaType);

        var script = Generator.GenerateCurl(Post(content), BaseAddress, null);

        script.Should().Contain("-d 'a=1'");
        script.Should().NotContain("--data-binary");
    }

    [Fact]
    public void FormUrlEncoded_Value_Containing_Ampersand_Stays_One_Field()
    {
        var content = new FormUrlEncodedContent([new("q", "a&b"), new("x", "1")]);

        var script = Generator.GenerateCurl(Post(content), BaseAddress, null);

        script.Should().Contain("--data-urlencode 'q=a&b' -d 'x=1'");
    }

    [Theory]
    [InlineData("1+1")]
    [InlineData("100%")]
    public void FormUrlEncoded_Value_With_Plus_Or_Percent_Is_Sent_Through_DataUrlEncode(string value)
    {
        var content = new FormUrlEncodedContent([new("k", value)]);

        var script = Generator.GenerateCurl(Post(content), BaseAddress, null);

        script.Should().Contain($"--data-urlencode 'k={value}'");
    }

    [Fact]
    public void FormUrlEncoded_Plain_Values_Keep_Using_Data()
    {
        var content = new FormUrlEncodedContent([new("a", "1"), new("b", "two words")]);

        var script = Generator.GenerateCurl(Post(content), BaseAddress, null);

        script.Should().Contain("-d 'a=1' -d 'b=two words'");
    }
}
