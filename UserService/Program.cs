using Microsoft.EntityFrameworkCore;
using UserService.Repository;
using UserService.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
