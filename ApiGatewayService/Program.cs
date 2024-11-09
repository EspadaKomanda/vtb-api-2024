using Serilog;
using Microsoft.EntityFrameworkCore;
using ApiGatewayService.Database;
using ApiGatewayService.Database.Models;
using ApiGatewayService.Utils;
using ApiGatewayService.Repositories;
using ApiGatewayService.Services.Account;
using ApiGatewayService.Services.Profile;

var builder = WebApplication.CreateBuilder(args);

Logging.configureLogging();

builder.Host.UseSerilog();

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();