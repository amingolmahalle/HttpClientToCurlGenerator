using System.Text;
using HttpClientToCurl.Config;
using HttpClientToCurl.Builder.Interface;

namespace HttpClientToCurl.Builder.Director;

public abstract class BaseBuilder : IBuilder
{
    protected readonly StringBuilder _stringBuilder = new();

    public abstract string CreateCurl(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config);
}
