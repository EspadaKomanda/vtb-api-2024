using Serilog;
using Microsoft.EntityFrameworkCore;
using PromoService.Database;
using PromoService.Utils;
using PromoService.Repositories;
using PromoService.Services.PromoApplication;
using PromoService.Services.Promocode;

var builder = WebApplication.CreateBuilder(args);

Logging.configureLogging();

builder.Host.UseSerilog();

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPromocodeService, PromocodeService>();
builder.Services.AddScoped<IPromoApplicationService, PromoApplicationService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();