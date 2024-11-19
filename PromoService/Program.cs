using Serilog;
using Microsoft.EntityFrameworkCore;
using PromoService.Database;
using PromoService.Utils;
using PromoService.Repositories;
using PromoService.Services.PromoApplication;
using PromoService.Services.Promocode;
using Confluent.Kafka;
using TourService.Kafka;
using PromoService.Database.Models;
using PromoService.KafkaServices;
using PromoService.Services.Users;

var builder = WebApplication.CreateBuilder(args);

Logging.configureLogging();

builder.Host.UseSerilog();

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddSingleton(new ProducerBuilder<string,string>(
    new ProducerConfig()
    {
        BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BROKERS") ?? "",
        Partitioner = Partitioner.Murmur2,
        CompressionType = Confluent.Kafka.CompressionType.None,
        ClientId= Environment.GetEnvironmentVariable("KAFKA_CLIENT_ID") ?? ""
    }
).Build());

builder.Services.AddSingleton(new ConsumerBuilder<string,string>(
    new ConsumerConfig()
    {
        BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BROKERS") ?? "",
        GroupId = Environment.GetEnvironmentVariable("KAFKA_CLIENT_ID") ?? "", 
        EnableAutoCommit = true,
        AutoCommitIntervalMs = 10,
        EnableAutoOffsetStore = true,
        AutoOffsetReset = AutoOffsetReset.Latest
    }
).Build());

builder.Services.AddSingleton(new AdminClientBuilder(
    new AdminClientConfig()
    {
        BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BROKERS")
    }
).Build());
builder.Services.AddSingleton<KafkaTopicManager>();
builder.Services.AddSingleton<KafkaRequestService>( sp => new KafkaRequestService(
    sp.GetRequiredService<IProducer<string,string>>(),
    sp.GetRequiredService<ILogger<KafkaRequestService>>(),
    sp.GetRequiredService<KafkaTopicManager>(),
    new List<string>(){
        Environment.GetEnvironmentVariable("USER_SERVICE_ACCOUNTS_RESPONSES") ?? "",
    },
    new List<string>(){
        Environment.GetEnvironmentVariable("USER_SERVICE_ACCOUNTS_REQUESTS") ?? "",
    }
));
builder.Services.AddScoped<IRepository<Promo>, Repository<Promo>>()
                .AddScoped<IRepository<PromoAppliedProduct>, Repository<PromoAppliedProduct>>()
                .AddScoped<IRepository<PromoCategory>, Repository<PromoCategory>>()
                .AddScoped<IRepository<UserPromo>, Repository<UserPromo>>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IPromocodeService, PromocodeService>();
builder.Services.AddScoped<IPromoApplicationService, PromoApplicationService>();

builder.Services.AddSingleton<KafkaPromocodeService>();
builder.Services.AddSingleton<KafkaPromoService>();

var app = builder.Build();
Thread thread = new Thread(async x=>{
    var kafkaPromocodeService = app.Services.GetService<KafkaPromocodeService>();
    await kafkaPromocodeService.Consume();
});

thread.Start();
Thread thread2 = new Thread(async x=>{
    var kafkaPromoService = app.Services.GetService<KafkaPromoService>();
    await kafkaPromoService.Consume();
});
thread2.Start();

Thread thread3 = new Thread( x=>{
    var kafkaRequestService = app.Services.GetService<KafkaRequestService>();
    kafkaRequestService.BeginRecieving(new List<string>(){
        Environment.GetEnvironmentVariable("USER_SERVICE_ACCOUNTS_RESPONSES") ?? "",
    });
});
thread3.Start();
app.UseHttpsRedirection();

app.Run();