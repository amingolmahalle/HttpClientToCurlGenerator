using HttpClientToCurl.Builder;
using HttpClientToCurl.Config;

namespace HttpRequestMessageToCurlTest.UnitTest.MediaTypes.Xml;

public class SuccessCurlGeneratorTests : SharedCurlTests.XmlSuccessCurlGeneratorTestsBase
{
    protected override string Generate(HttpRequestMessage request, Uri? baseAddress, BaseConfig? config)
        => Generator.GenerateCurl(request, baseAddress!, config!);
}
