using Amazon;
using Amazon.S3;
using Confluent.Kafka;
using EntertaimentService.Database;
using EntertaimentService.Database.Models;
using EntertaimentService.Kafka;
using EntertaimentService.KafkaServices;
using EntertaimentService.Services.BenefitService;
using EntertaimentService.Services.CategoryService;
using EntertaimentService.Services.EntertaimentServices;
using EntertaimentService.Services.PaymentMethodService;
using EntertaimentService.Services.PaymentVariantService;
using EntertaimentService.Services.PhotoService;
using EntertaimentService.Services.ReviewService;
using EntertaimentService.Services.S3;
using EntertaimentService.Services.WishlistService;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TourService.Kafka;
using TourService.KafkaServices;
using TourService.Services.PaymentMethodService;
using TourService.Services.WishlistService;
using TourService.Utils;
using UserService.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
Logging.configureLogging();

builder.Host.UseSerilog();
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
        Environment.GetEnvironmentVariable("USER_RESPONSE_TOPIC") ?? "",
    },
    new List<string>(){
        Environment.GetEnvironmentVariable("USER_REQUEST_TOPIC") ?? "",
    }
));
builder.Services.AddScoped<IRepository<Benefit>, Repository<Benefit>>()
                .AddScoped<IRepository<Category>, Repository<Category>>()
                .AddScoped<IRepository<Entertaiment>, Repository<Entertaiment>>()
                .AddScoped<IRepository<EntertaimentCategory>, Repository<EntertaimentCategory>>()
                .AddScoped<IRepository<EntertaimentPaymentMethod>, Repository<EntertaimentPaymentMethod>>()
                .AddScoped<IRepository<EntertaimentWish>, Repository<EntertaimentWish>>()
                .AddScoped<IRepository<PaymentMethod>, Repository<PaymentMethod>>()
                .AddScoped<IRepository<PaymentVariant>, Repository<PaymentVariant>>()
                .AddScoped<IRepository<Photo>, Repository<Photo>>()
                .AddScoped<IRepository<Review>, Repository<Review>>()
                .AddScoped<IRepository<ReviewBenefit>, Repository<ReviewBenefit>>()
                .AddScoped<IRepository<ReviewFeedback>, Repository<ReviewFeedback>>()
                .AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IBenefitService, BenefitService>()
                .AddScoped<ICategoryService, CategoryService>()
                .AddScoped<IEntertaimentService, EntertaimentService.Services.EntertaimentServices.EntertaimentService>()
                .AddScoped<IPaymentMethodService, PaymentMethodService>()
                .AddScoped<IPaymentVariantService, PaymentVariantService>()
                .AddScoped<IReviewService, ReviewService>()
                .AddScoped<IPhotoService, PhotoService>()
                .AddScoped<IS3Service, S3Service>()
                .AddScoped<IWishlistService, WishlistService>();




builder.Services.AddSingleton<KafkaBenefitService>();
builder.Services.AddSingleton<KafkaCegoryService>();
builder.Services.AddSingleton<KafkaEntertaimentService>();
builder.Services.AddSingleton<KafkaPaymentMethodService>();
builder.Services.AddSingleton<KafkaPaymentVariantService>();
builder.Services.AddSingleton<KafkaPhotoService>();
builder.Services.AddSingleton<KafkaReviewService>();
builder.Services.AddSingleton<KafkaWishlistService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
Thread thread = new(async () => {
    var kafkaBenefitService = app.Services.GetRequiredService<KafkaBenefitService>();
   
    await kafkaBenefitService.Consume();
});
thread.Start();
Thread thread1 = new(async () => {
    var kafkaCegoryService = app.Services.GetRequiredService<KafkaCegoryService>();
    await kafkaCegoryService.Consume();
});
thread1.Start();
Thread thread2 = new(async () => {
    var kafkaEntertaimentService = app.Services.GetRequiredService<KafkaEntertaimentService>();
    await kafkaEntertaimentService.Consume();
});
thread2.Start();
Thread thread3 = new(async () => {
    var kafkaPaymentMethodService = app.Services.GetRequiredService<KafkaPaymentMethodService>();
    await kafkaPaymentMethodService.Consume();
});
thread3.Start();
Thread thread4 = new(async () => {
    var kafkaPaymentVariantService = app.Services.GetRequiredService<KafkaPaymentVariantService>();
    await kafkaPaymentVariantService.Consume();
});
thread4.Start();
Thread thread5 = new(async () => {
    var kafkaPhotoService = app.Services.GetRequiredService<KafkaPhotoService>();
    await kafkaPhotoService.Consume();
});
thread5.Start();
Thread thread6 = new(async () => {
    var kafkaReviewService = app.Services.GetRequiredService<KafkaReviewService>();
    await kafkaReviewService.Consume();
});
thread6.Start();
Thread thread7 = new(async () => {
    var kafkaWishlistService = app.Services.GetRequiredService<KafkaWishlistService>();
    await kafkaWishlistService.Consume();
});
thread7.Start();
Thread thread8 = new Thread( x =>{
    var kafkaRequestService = app.Services.GetRequiredService<KafkaRequestService>();
    kafkaRequestService.BeginRecieving(new List<string>(){
       
        Environment.GetEnvironmentVariable("USER_RESPONSE_TOPIC") ?? "",
    
    });  
});
thread8.Start();
app.UseHttpsRedirection();


app.Run();
