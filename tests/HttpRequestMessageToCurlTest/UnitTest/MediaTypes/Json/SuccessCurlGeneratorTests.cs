using HttpClientToCurl.Builder;
using HttpClientToCurl.Config;

namespace HttpRequestMessageToCurlTest.UnitTest.MediaTypes.Json;

public class SuccessCurlGeneratorTests : SharedCurlTests.JsonSuccessCurlGeneratorTestsBase
{
    protected override string Generate(HttpRequestMessage request, Uri? baseAddress, BaseConfig? config)
        => Generator.GenerateCurl(request, baseAddress!, config!);
}
