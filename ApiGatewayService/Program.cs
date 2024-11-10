using Serilog;
using Microsoft.EntityFrameworkCore;
using ApiGatewayService.Utils;
using Microsoft.OpenApi.Models;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Сервис генерации изображений",
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Аутентификация при помощи токена типа Access и Refresh.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        BearerFormat = "<type> <token>",
        Type = SecuritySchemeType.ApiKey
    });
    var xmlFile = Path.Combine(AppContext.BaseDirectory, "TestAPI.xml");
    if (File.Exists(xmlFile))
    {
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

Logging.configureLogging();

builder.Host.UseSerilog();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Access", policy =>
    {
        policy.RequireClaim("TokenType", "access");
    })
    .AddPolicy("Refresh", policy =>
    {
        policy.RequireClaim("TokenType", "refresh");
    });

builder.Services.AddCors(options => 
{
    options.AddDefaultPolicy(builder => 
    {
        builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddAuthentication("default");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();
app.UseHttpsRedirection();

app.UseCors();
app.Run();