using MailService.Services.Mailer;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// TODO: add configuration

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<IMailerService, MailerService>();

app.Run();