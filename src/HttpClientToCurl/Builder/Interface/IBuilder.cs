using HttpClientToCurl.Config;

namespace HttpClientToCurl.Builder.Interface;

public interface IBuilder
{
    string Create(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config);
}
