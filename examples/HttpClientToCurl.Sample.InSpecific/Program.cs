using HttpClientToCurl.Config.Others;
using HttpClientToCurl.HttpMessageHandlers;
using static HttpClientToCurl.Extensions.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHttpClientToCurl(config =>
{
    config.ShowMode = ShowMode.Console | ShowMode.File;
    config.NeedAddDefaultHeaders = true;
    config.ConsoleEnableCodeBeautification = true;
    config.FileConfigPath = "C:\\Users\\Public";
    config.FileConfigFileName = "curl_commands";
}, false);
builder.Services.AddHttpClient("my-client")
                .AddHttpMessageHandler<CurlGeneratorHttpMessageHandler>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
