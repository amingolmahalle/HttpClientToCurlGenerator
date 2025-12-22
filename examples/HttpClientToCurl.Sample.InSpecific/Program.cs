using static HttpClientToCurl.Extensions.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddHttpClientToCurl(builder.Configuration);
builder.Services.AddHttpClient("my-client1").AddCurlLogging();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
