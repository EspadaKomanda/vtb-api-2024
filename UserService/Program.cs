using Serilog;
using Microsoft.EntityFrameworkCore;
using UserService.Database;
using UserService.Database.Models;
using UserService.Utils;
using UserService.Repositories;
using UserService.Services.Account;
using UserService.Services.Profile;
using Confluent.Kafka;
using Amazon.S3;
using Amazon;
using TourService.Kafka;
using EntertaimentService.Services.S3;
using UserService.KafkaServices;

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

builder.Services.AddSingleton<IAmazonS3>(sc =>
{
    var awsS3Config = new AmazonS3Config
    {
        RegionEndpoint = RegionEndpoint.USEast1,
        ServiceURL = Environment.GetEnvironmentVariable("S3_URL") ?? "",
        ForcePathStyle = true
    };

    return new AmazonS3Client("s3manager","s3manager",awsS3Config);
});
builder.Services.AddSingleton<KafkaTopicManager>();
builder.Services.AddSingleton<KafkaRequestService>( sp => new KafkaRequestService(
    sp.GetRequiredService<IProducer<string,string>>(),
    sp.GetRequiredService<ILogger<KafkaRequestService>>(),
    sp.GetRequiredService<KafkaTopicManager>(),
    new List<string>(){
        Environment.GetEnvironmentVariable("MAIL_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("DATA_CACHE_RESPONSE_TOPIC") ?? "",
    },
    new List<string>(){
        Environment.GetEnvironmentVariable("MAIL_REQUEST_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("DATA_CACHE_QUEST_TOPIC") ?? "",
    }
));
builder.Services.AddScoped<IRepository<User>, Repository<User>>()
                .AddScoped<IRepository<Meta>, Repository<Meta>>()
                .AddScoped<IRepository<PersonalData>, Repository<PersonalData>>()
                .AddScoped<IRepository<RegistrationCode>, Repository<RegistrationCode>>()
                .AddScoped<IRepository<Role>, Repository<Role>>()
                .AddScoped<IRepository<VisitedTour>, Repository<VisitedTour>>()
                .AddScoped<IRepository<ResetCode>, Repository<ResetCode>>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProfileService, ProfileService>();



builder.Services.AddScoped<IAccountService, AccountService>()
                .AddScoped<IProfileService, ProfileService>()
                .AddScoped<IS3Service, S3Service>();

builder.Services.AddSingleton<AccountKafkaService>();
builder.Services.AddSingleton<ProfileKafkaService>();
var app = builder.Build();

Thread thread = new Thread(async x =>
{
    var AccountKafkaService = app.Services.GetRequiredService<AccountKafkaService>();
    await AccountKafkaService.Consume();
});
thread.Start();

Thread thread2 = new Thread(async x =>
{
    var ProfileKafkaService = app.Services.GetRequiredService<ProfileKafkaService>();
    await ProfileKafkaService.Consume();
});
thread2.Start();

Thread thread3 = new Thread( x =>
{
    var KafkaRequestService = app.Services.GetRequiredService<KafkaRequestService>();
    KafkaRequestService.BeginRecieving(new List<string>(){
        Environment.GetEnvironmentVariable("MAIL_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("DATA_CACHE_RESPONSE_TOPIC") ?? "",
    });
});
thread3.Start();
app.UseHttpsRedirection();

app.Run();