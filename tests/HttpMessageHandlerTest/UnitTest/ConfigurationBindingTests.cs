using System.Net;
using FluentAssertions;
using HttpClientToCurl.Extensions;
using HttpClientToCurl.HttpMessageHandlers;
using HttpMessageHandlerTest.UnitTest.Fakes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HttpMessageHandlerTest.UnitTest;

/// <summary>
/// Exercises the path users actually take: appsettings section -> DI registration -> HttpClient pipeline.
/// </summary>
public class ConfigurationBindingTests
{
    private static IConfiguration RedactingConfiguration() => new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["HttpClientToCurl:Enable"] = "true",
            ["HttpClientToCurl:SendToLogger:TurnOn"] = "true",
            ["HttpClientToCurl:SendToLogger:LogLevel"] = "Information",
            ["HttpClientToCurl:SendToLogger:RedactedHeaders:0"] = "Authorization",
        })
        .Build();

    private static async Task<FakeLogger<CurlGeneratorHttpMessageHandler>> SendThrough(
        Action<IServiceCollection, IConfiguration> register, Action<IHttpClientBuilder>? customizeClient = null)
    {
        var logger = new FakeLogger<CurlGeneratorHttpMessageHandler>();
        var services = new ServiceCollection();
        services.AddSingleton<ILogger<CurlGeneratorHttpMessageHandler>>(logger);

        register(services, RedactingConfiguration());
        var clientBuilder = services.AddHttpClient("test")
            .ConfigurePrimaryHttpMessageHandler(() => new FakeHttpMessageHandler());
        customizeClient?.Invoke(clientBuilder);

        var provider = services.BuildServiceProvider();
        var client = provider.GetRequiredService<IHttpClientFactory>().CreateClient("test");
        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/test");
        request.Headers.Add("Authorization", "Bearer super-secret");

        var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return logger;
    }

    [Fact]
    public async Task Global_Registration_Redacts_Headers_From_Configuration()
    {
        var logger = await SendThrough((services, config) => services.AddAllHttpClientToCurl(config));

        var message = logger.LoggedMessages.Should().ContainSingle(m => m.Message.StartsWith("curl")).Subject.Message;
        message.Should().Contain("Authorization: [REDACTED]").And.NotContain("super-secret");
    }

    [Fact]
    public async Task PerClient_Registration_Redacts_Headers_From_Configuration()
    {
        var logger = await SendThrough(
            (services, config) => services.AddHttpClientToCurl(config),
            clientBuilder => clientBuilder.AddCurlLogging());

        var message = logger.LoggedMessages.Should().ContainSingle(m => m.Message.StartsWith("curl")).Subject.Message;
        message.Should().Contain("Authorization: [REDACTED]").And.NotContain("super-secret");
    }

    [Fact]
    public async Task NeedAddDefaultHeaders_False_Has_No_Effect_In_Automatic_Mode()
    {
        // Documented on BaseConfig.NeedAddDefaultHeaders: HttpClient merges its default headers into the
        // request before the handler runs, so the handler always sees (and prints) them.
        var logger = new FakeLogger<CurlGeneratorHttpMessageHandler>();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["HttpClientToCurl:SendToLogger:TurnOn"] = "true",
                ["HttpClientToCurl:SendToLogger:LogLevel"] = "Information",
                ["HttpClientToCurl:SendToLogger:NeedAddDefaultHeaders"] = "false",
            })
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<ILogger<CurlGeneratorHttpMessageHandler>>(logger);
        services.AddAllHttpClientToCurl(configuration);
        services.AddHttpClient("test", client => client.DefaultRequestHeaders.Add("X-Default", "v"))
            .ConfigurePrimaryHttpMessageHandler(() => new FakeHttpMessageHandler());

        var client = services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>().CreateClient("test");
        await client.GetAsync("http://localhost/test");

        logger.LoggedMessages.Should().ContainSingle(m => m.Message.StartsWith("curl"))
            .Which.Message.Should().Contain("X-Default: v");
    }
}
