using HttpClientToCurl.Config;

namespace HttpClientToCurl.Builder.Director;

public class Creator(Interface.IBuilder builder)
{
    public string CreateCurl(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config)
    {
        return builder.Create(httpClient, httpRequestMessage, config);
    }
}
