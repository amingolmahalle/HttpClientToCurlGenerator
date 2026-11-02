using HttpClientToCurl.Builder;
using HttpClientToCurl.Config;

namespace HttpRequestMessageToCurlTest.UnitTest.MediaTypes.Json;

public class MethodHandlingTests : SharedCurlTests.MethodHandlingTestsBase
{
    protected override string Generate(HttpRequestMessage request, Uri? baseAddress, BaseConfig? config)
        => Generator.GenerateCurl(request, baseAddress!, config!);
}
