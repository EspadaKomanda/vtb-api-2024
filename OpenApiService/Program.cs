using OpenApiService.Utils;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Logging.configureLogging();
builder.Host.UseSerilog();
var app = builder.Build();


app.Run();
