using HttpClientToCurl;
using Xunit;

namespace HttpClientToCurlGeneratorTest.UnitTest;

public class CurlGeneratorTests
{
    [Theory]
    [InlineData("my-scripts","")]
    [InlineData(null,null)]
    public void SuccessGenerateCurlInFile(string filename, string path)
    {
        HttpClient httpClient = new HttpClient();
        
        httpClient.GenerateCurlInFile(null, null,
            configs =>
            {
                configs.Filename = filename;
                configs.Path = path;
                configs.TurnOn = false;
                configs.NeedAddDefaultHeaders = true;
            });
    }
}