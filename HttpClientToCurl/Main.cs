namespace HttpClientToCurl;

public static class Main
{
    #region :: Extensions ::

    public static void GenerateCurlInConsole(
        this HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        string requestUri,
        bool needAddDefaultHeaders = true,
        bool turnOn = true)
    {
        if (!turnOn)
            return;

        string script = Generator.GenerateCurl(httpClient, httpRequestMessage, requestUri, needAddDefaultHeaders);

        Utility._WriteInConsole(script);
    }

    public static void GenerateCurlInFile(
        this HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        string requestUri,
        bool needAddDefaultHeaders = true,
        bool turnOn = true)
    {
        if (!turnOn)
            return;

        string script = Generator.GenerateCurl(httpClient, httpRequestMessage, requestUri, needAddDefaultHeaders);

        Utility._WriteInFile(script);
    }

    #endregion
}