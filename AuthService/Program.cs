using Serilog;
using AuthService.Utils;
using AuthService.Services.Jwt;
using AuthService.Services.Authentication;
using AuthService.Services.AccessDataCache;
using Confluent.Kafka;
using AuthService.KafkaServices;
using TourService.Kafka;

var builder = WebApplication.CreateBuilder(args);

Logging.configureLogging();

builder.Host.UseSerilog();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
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
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = builder.Configuration["RedisCacheOptions:Configuration"];
    options.InstanceName  = builder.Configuration["RedisCacheOptions:InstanceName"];
});

builder.Services.AddScoped<IAccessDataCacheService, AccessDataCacheService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddSingleton<KafkaTopicManager>();
builder.Services.AddSingleton<KafkaRequestService>( sp => new KafkaRequestService(
    sp.GetRequiredService<IProducer<string,string>>(),
    sp.GetRequiredService<ILogger<KafkaRequestService>>(),
    sp.GetRequiredService<KafkaTopicManager>(),
    new List<string>(){
        "user-service-accounts-responses",
    },
    new List<string>(){
        "user-service-accounts-requests",
    }
));
builder.Services.AddSingleton<KafkaAccesDataCacheService>()
                .AddSingleton<KafkaAuthService>();
var app = builder.Build();

Thread thread = new(async () => {
    var kafkaAccesDataCacheService = app.Services.GetRequiredService<KafkaAccesDataCacheService>();
   
    await kafkaAccesDataCacheService.Consume();
});
thread.Start();
Thread thread1 = new(async () => {
    var kafkaAuthService = app.Services.GetRequiredService<KafkaAuthService>();
    await kafkaAuthService.Consume();
});
Thread thread2 = new(x => {
   var KafkaRequestService = app.Services.GetRequiredService<KafkaRequestService>();
   KafkaRequestService.BeginRecieving(new List<string>(){
    "user-service-accounts-responses"
   });
});
thread1.Start();
thread2.Start();
app.Run();