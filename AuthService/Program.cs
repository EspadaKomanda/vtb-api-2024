using Serilog;
using AuthService.Utils;

var builder = WebApplication.CreateBuilder(args);

Logging.configureLogging();

builder.Host.UseSerilog();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();