using HttpClientToCurl.Builder;
using HttpClientToCurl.Config;

namespace HttpClientToCurlTest.UnitTest.MediaTypes.Xml;

public class SuccessCurlGeneratorTests : SharedCurlTests.XmlSuccessCurlGeneratorTestsBase
{
    protected override string Generate(HttpRequestMessage request, Uri? baseAddress, BaseConfig? config)
    {
        using var client = new HttpClient { BaseAddress = baseAddress };
        return Generator.GenerateCurl(client, request, config!);
    }
}
