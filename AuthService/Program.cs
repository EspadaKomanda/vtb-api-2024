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

builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = builder.Configuration["RedisCacheOptions:Configuration"];
    options.InstanceName  = builder.Configuration["RedisCacheOptions:InstanceName"];
});

builder.Services.AddScoped<IAccessDataCacheService, AccessDataCacheService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

app.Run();