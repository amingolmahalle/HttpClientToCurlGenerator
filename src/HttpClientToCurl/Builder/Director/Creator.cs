using HttpClientToCurl.Config;
using HttpClientToCurl.Builder.Interface;

namespace HttpClientToCurl.Builder.Director;

public class Creator(IBuilder builder)
{
    public string CreateCurl(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config)
    {
        return builder.Create(httpClient, httpRequestMessage, config);
    }
}
