using Serilog;
using Microsoft.EntityFrameworkCore;
using PromoService.Database;
using PromoService.Utils;
using PromoService.Repositories;

var builder = WebApplication.CreateBuilder(args);

Logging.configureLogging();

builder.Host.UseSerilog();

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();