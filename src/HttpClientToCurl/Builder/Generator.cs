using HttpClientToCurl.Builder.Concrete;
using HttpClientToCurl.Builder.Interface;
using HttpClientToCurl.Config;

namespace HttpClientToCurl.Builder;

public static class Generator
{
    public static string GenerateCurl(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config)
    {
        var builder = GetBuilder(httpRequestMessage.Method);
        return builder.CreateCurl(httpClient, httpRequestMessage, config);
    }

    private static IBuilder GetBuilder(HttpMethod method)
    {
        string methodName = method.Method;
        return methodName switch
        {
            "GET" => new HttpGetBuilder(),
            "POST" => new HttpPostBuilder(),
            "PUT" => new HttpPutBuilder(),
            "PATCH" => new HttpPatchBuilder(),
            "DELETE" => new HttpDeleteBuilder(),
            _ => throw new NotSupportedException($"HTTP method {method} is not supported."),
        };
    }
}
