using static HttpClientToCurl.Extensions.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpClientToCurl(config =>
{
    config.TurnOnAll = true;
    config.ShowOnConsole = consoleConfig =>
    {
        consoleConfig.TurnOn = true;
        consoleConfig.EnableCodeBeautification = true;
    };
    config.SaveToFile = fileConfig =>
    {
        fileConfig.TurnOn = true;
        fileConfig.Filename = "curl_commands.txt";
        fileConfig.Path = "C:\\logs";
        fileConfig.NeedAddDefaultHeaders = true;
    };
});
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
