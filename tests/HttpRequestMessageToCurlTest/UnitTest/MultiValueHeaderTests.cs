using HttpClientToCurl.Builder;
using HttpClientToCurl.Config;

namespace HttpRequestMessageToCurlTest.UnitTest;

public class MultiValueHeaderTests : SharedCurlTests.MultiValueHeaderTestsBase
{
    protected override string Generate(HttpRequestMessage request, Uri? baseAddress, BaseConfig? config)
        => Generator.GenerateCurl(request, baseAddress!, config!);
}
