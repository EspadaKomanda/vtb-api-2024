using Serilog;
using AuthService.Utils;
using AuthService.Services.Jwt;
using AuthService.Services.Authentication;
using AuthService.Services.AccessDataCache;

var builder = WebApplication.CreateBuilder(args);

Logging.configureLogging();

builder.Host.UseSerilog();

var app = builder.Build();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddScoped<IJwtService, JwtService>();
// TODO: implementation :D
//builder.Services.AddScoped<IAccessDataCacheService, AccessDataCacheService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

app.Run();