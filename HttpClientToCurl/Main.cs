﻿using System.Net.Http.Headers;
using HttpClientToCurl.Config;
using HttpClientToCurl.Utility;

namespace HttpClientToCurl;

public static class Main
{
    #region :: EXTENSIONS ::

    public static string GenerateCurlInString(
        this HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        string requestUri = null,
        Action<StringConfig> config = null)
    {
        var stringConfig = new StringConfig();
        config?.Invoke(stringConfig);

        if (!stringConfig.TurnOn)
            return string.Empty;

        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            string.IsNullOrWhiteSpace(requestUri)
                ? httpRequestMessage.RequestUri?.ToString()
                : requestUri,
            stringConfig.NeedAddDefaultHeaders);

        return script;
    }

    public static string GenerateCurlInString(
        this HttpClient httpClient,
        HttpMethod httpMethod,
        string requestUri = null,
        HttpRequestHeaders requestHeaders = null,
        HttpContent requestBody = null,
        Action<StringConfig> config = null)
    {
        var stringConfig = new StringConfig();
        config?.Invoke(stringConfig);

        if (!stringConfig.TurnOn)
            return string.Empty;

        var httpRequestMessage = Helpers.FillHttpRequestMessage(httpMethod, requestHeaders, requestBody);

        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            string.IsNullOrWhiteSpace(requestUri)
                ? httpRequestMessage.RequestUri?.ToString()
                : requestUri,
            stringConfig.NeedAddDefaultHeaders);

        return script;
    }

    public static void GenerateCurlInConsole(
        this HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        string requestUri = null,
        Action<ConsoleConfig> config = null)
    {
        var consoleConfig = new ConsoleConfig();
        config?.Invoke(consoleConfig);

        if (!consoleConfig.TurnOn)
            return;

        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            string.IsNullOrWhiteSpace(requestUri)
                ? httpRequestMessage.RequestUri?.ToString()
                : requestUri,
            consoleConfig.NeedAddDefaultHeaders);

        Helpers.WriteInConsole(script, consoleConfig.EnableCodeBeautification, httpRequestMessage.Method);
    }

    public static void GenerateCurlInConsole(
        this HttpClient httpClient,
        HttpMethod httpMethod,
        string requestUri = null,
        HttpRequestHeaders requestHeaders = null,
        HttpContent requestBody = null,
        Action<ConsoleConfig> config = null)
    {
        var consoleConfig = new ConsoleConfig();
        config?.Invoke(consoleConfig);

        if (!consoleConfig.TurnOn)
            return;

        var httpRequestMessage = Helpers.FillHttpRequestMessage(httpMethod, requestHeaders, requestBody);

        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            string.IsNullOrWhiteSpace(requestUri)
                ? httpRequestMessage.RequestUri?.ToString()
                : requestUri,
            consoleConfig.NeedAddDefaultHeaders);

        Helpers.WriteInConsole(script, consoleConfig.EnableCodeBeautification, httpRequestMessage.Method);
    }

    public static void GenerateCurlInFile(
        this HttpClient httpClient,
        HttpRequestMessage httpRequestMessage,
        string requestUri = null,
        Action<FileConfig> config = null)
    {
        var fileConfig = new FileConfig();
        config?.Invoke(fileConfig);

        if (!fileConfig.TurnOn)
            return;

        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            string.IsNullOrWhiteSpace(requestUri)
                ? httpRequestMessage.RequestUri?.ToString()
                : requestUri,
            fileConfig.NeedAddDefaultHeaders);

        Helpers.WriteInFile(script, fileConfig.Filename, fileConfig.Path);
    }

    public static void GenerateCurlInFile(
        this HttpClient httpClient,
        HttpMethod httpMethod,
        string requestUri = null,
        HttpRequestHeaders requestHeaders = null,
        HttpContent requestBody = null,
        Action<FileConfig> config = null)
    {
        var fileConfig = new FileConfig();
        config?.Invoke(fileConfig);

        if (!fileConfig.TurnOn)
            return;

        var httpRequestMessage = Helpers.FillHttpRequestMessage(httpMethod, requestHeaders, requestBody);

        string script = Generator.GenerateCurl(
            httpClient,
            httpRequestMessage,
            string.IsNullOrWhiteSpace(requestUri)
                ? httpRequestMessage.RequestUri?.ToString()
                : requestUri,
            fileConfig.NeedAddDefaultHeaders);

        Helpers.WriteInFile(script, fileConfig.Filename, fileConfig.Path);
    }

    #endregion
}