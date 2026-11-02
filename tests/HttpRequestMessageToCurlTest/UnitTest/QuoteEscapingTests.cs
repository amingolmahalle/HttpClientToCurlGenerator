using HttpClientToCurl.Builder;
using HttpClientToCurl.Config;

namespace HttpRequestMessageToCurlTest.UnitTest;

public class QuoteEscapingTests : SharedCurlTests.QuoteEscapingTestsBase
{
    protected override string Generate(HttpRequestMessage request, Uri? baseAddress, BaseConfig? config)
        => Generator.GenerateCurl(request, baseAddress!, config!);
}
