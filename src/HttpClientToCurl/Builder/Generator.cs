using System.Data;
using HttpClientToCurl.Builder.Concrete;
using HttpClientToCurl.Builder.Director;
using HttpClientToCurl.Config;

namespace HttpClientToCurl.Builder;

public static class Generator
{
    public static string GenerateCurl(HttpClient httpClient, HttpRequestMessage httpRequestMessage, BaseConfig config)
    {
        string script;

        try
        {
            if (httpRequestMessage.Method == HttpMethod.Get)
            {
                var instance = new Creator(new HttpGetBuilder());
                script = instance.CreateCurl(httpClient, httpRequestMessage, config);
            }
            else if (httpRequestMessage.Method == HttpMethod.Post)
            {
                var instance = new Creator(new HttpPostBuilder());
                script = instance.CreateCurl(httpClient, httpRequestMessage, config);
            }
            else if (httpRequestMessage.Method == HttpMethod.Put)
            {
                var instance = new Creator(new HttpPutBuilder());
                script = instance.CreateCurl(httpClient, httpRequestMessage, config);
            }
            else if (httpRequestMessage.Method == HttpMethod.Patch)
            {
                var instance = new Creator(new HttpPatchBuilder());
                script = instance.CreateCurl(httpClient, httpRequestMessage, config);
            }
            else if (httpRequestMessage.Method == HttpMethod.Delete)
            {
                var instance = new Creator(new HttpDeleteBuilder());
                script = instance.CreateCurl(httpClient, httpRequestMessage, config);
            }
            else
            {
                throw new DataException($"not supported {httpRequestMessage.Method.Method} by HttpClientToCurl!");
            }
        }
        catch (Exception exception)
        {
            script = $"GenerateCurlError => {exception.Message} {exception.InnerException}";
        }

        return script;
    }
}
