using static HttpClientToCurl.Extensions.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddHttpClientToCurlInGeneralMode(builder.Configuration);
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
