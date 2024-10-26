using Microsoft.EntityFrameworkCore;
using UserService.Repository;
using UserService.Database;
using UserService.Database.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Role>, Repository<Role>>();
builder.Services.AddScoped<IRepository<Meta>, Repository<Meta>>();
builder.Services.AddScoped<IRepository<VisitedTour>, Repository<VisitedTour>>();
builder.Services.AddScoped<IRepository<RegistrationCode>, Repository<RegistrationCode>>();
builder.Services.AddScoped<IRepository<PersonalData>, Repository<PersonalData>>();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
