using Serilog;
using Microsoft.EntityFrameworkCore;
using ApiGatewayService.Utils;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using Confluent.Kafka;
using TourService.Kafka;
using AuthService.Services.AccessDataCache;
using ApiGatewayService.sServices.AuthService.AccessDataCache;
using ApiGatewayService.Services.AuthService.Auth;
using ApiGatewayService.Services.EntertaimentService.Benefits;
using ApiGatewayService.Services.EntertaimentService.Categories;
using ApiGatewayService.Services.EntertaimentService.Entertaiments;
using ApiGatewayService.Services.EntertaimentService.PaymentMethods;
using ApiGatewayService.Services.EntertaimentService.PaymentVariants;
using ApiGatewayService.Services.EntertaimentService.Photos;
using ApiGatewayService.Services.EntertaimentService.Reviews;
using ApiGatewayService.Services.EntertaimentService.Wishlist;
using ApiGatewayService.Services.Jwt;
using ApiGatewayService.Services.PromoService.PromoApplication;
using ApiGatewayService.Services.PromoService;
using ApiGatewayService.Services.TourService.Benefits;
using ApiGatewayService.Services.TourService.Categories;
using ApiGatewayService.Services.TourService.PaymentMethods;
using ApiGatewayService.Services.TourService.PaymentVariants;
using ApiGatewayService.Services.TourService.Tours;
using ApiGatewayService.Services.TourService.Photos;
using ApiGatewayService.Services.TourService.Reviews;
using ApiGatewayService.Services.TourService.Tags;
using ApiGatewayService.Services.TourService.Wishlist;
using ApiGatewayService.Services.UserService.Account;
using ApiGatewayService.Services.UserService.Profile;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Сервис генерации изображений",
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Аутентификация при помощи токена типа Access и Refresh.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        BearerFormat = "<type> <token>",
        Type = SecuritySchemeType.ApiKey
    });
    var xmlFile = Path.Combine(AppContext.BaseDirectory, "TestAPI.xml");
    if (File.Exists(xmlFile))
    {
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

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
builder.Services.AddSingleton<KafkaTopicManager>();
builder.Services.AddSingleton<KafkaRequestService>( sp => new KafkaRequestService(
    sp.GetRequiredService<IProducer<string,string>>(),
    sp.GetRequiredService<ILogger<KafkaRequestService>>(),
    sp.GetRequiredService<KafkaTopicManager>(),
    new List<string>(){
        Environment.GetEnvironmentVariable("AUTH_ACCESS_DATA_CACHE_RESPONSES") ?? "",
        Environment.GetEnvironmentVariable("AUTH_RESPONSES") ?? "",
        Environment.GetEnvironmentVariable("BENEFIT_ENTERTAIMENT_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_CATEGORY_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_PAYMENT_METHOD_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_PAYMENT_VARIANT_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_PHOTOS_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_REVIEW_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_WISHLIST_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("PROMO_APPLICATION_RESPONSES") ?? "",
        Environment.GetEnvironmentVariable("BENEFIT_TOUR_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_CATEGORY_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_PAYMENT_METHODS_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_PAYMENT_VARIANTS_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_PHOTOS_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_REVIEWS_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_TAGS_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_WISHLIST_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("USER_SERVICE_ACCOUNTS_RESPONSES") ?? "",
        Environment.GetEnvironmentVariable("USER_SERVICE_PROFILE_RESPONSES") ?? "",
    },
    new List<string>(){
        Environment.GetEnvironmentVariable("AUTH_ACCESS_DATA_CACHE_REQUESTS") ?? "",
        Environment.GetEnvironmentVariable("AUTH_REQUESTS") ?? "",
        Environment.GetEnvironmentVariable("BENEFIT_ENTERTAIMENT_REQUEST_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_CATEGORY_REQUEST_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_REQUEST_TOPIC")  ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_PAYMENT_METHOD_REQUEST_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_PAYMENT_VARIANT_REQUEST_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_PHOTOS_REQUEST_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_REVIEW_REQUEST_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_WISHLIST_REQUEST_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("PROMO_APPLICATION_REQUESTS") ?? "",
        Environment.GetEnvironmentVariable("BENEFIT_TOUR_REQUEST_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_CATEGORY_REQUEST_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_PAYMENT_METHODS_REQUEST_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_PAYMENT_VARIANTS_REQUEST_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_PHOTOS_REQUEST_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_REVIEWS_REQUEST_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_TAGS_REQUEST_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_REQUEST_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_WISHLIST_REQUEST_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("USER_SERVICE_ACCOUNTS_REQUESTS") ?? "",
        Environment.GetEnvironmentVariable("USER_SERVICE_PROFILE_REQUESTS") ?? "",
    }
));
builder.Services.AddScoped<IAccessDataCacheService,AccessDataCacheService>()
                .AddScoped<IAuthService,ApiGatewayService.Services.AuthService.Auth.AuthService>()
                .AddScoped<IEntertaimentBenefitService,EntertaimentBenefitService>()
                .AddScoped<IEntertaimentCategoryService,EntertaimentCategoryService>()
                .AddScoped<IEntertaimentService,ApiGatewayService.Services.EntertaimentService.Entertaiments.EntertaimentService>()
                .AddScoped<IEntertaimentPaymentMethodService,EntertaimentPaymentMethodService>()
                .AddScoped<IEntertaimentPaymentVariant,EntertaimentPaymentVariant>()
                .AddScoped<IEntertaimentPhotoService,EntertaimentPhotoService>()
                .AddScoped<IEntertaimentReviewsService,EntertaimentReviewsService>()
                .AddScoped<IEntertaimentWishlistService,EntertaimentWishlistService>()
                .AddScoped<IJwtService,JwtService>()
                .AddScoped<IPromoApplicationService,PromoApplicationService>()
                .AddScoped<ITourBenefitsService,TourBenefitsService>()
                .AddScoped<ITourCategoryService,TourCategoryService>()
                .AddScoped<ITourPaymentMethodsService,TourPaymentMethodsService>()
                .AddScoped<ITourPaymentVariantsService,TourPaymentVariantsService>()
                .AddScoped<ITourPhotoService,TourPhotoService>()
                .AddScoped<ITourReviewsService,TourReviewsService>()
                .AddScoped<ITourTagsService,TourTagsService>()
                .AddScoped<ITourService,ApiGatewayService.Services.TourService.Tours.TourService>()
                .AddScoped<ITourWishlistService,TourWishlistService>()
                .AddScoped<IAccountService,AccountService>()
                .AddScoped<IProfileService,ProfileService>();

builder.Services.AddControllers();


builder.Services.AddCors(options => 
{
    options.AddDefaultPolicy(builder => 
    {
        builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddAuthentication("default");

var app = builder.Build();  
Thread thread = new Thread( x=>{
    var KafkaRequestService = app.Services.GetRequiredService<KafkaRequestService>();
    KafkaRequestService.BeginRecieving(new List<string>(){
        Environment.GetEnvironmentVariable("AUTH_ACCESS_DATA_CACHE_RESPONSES") ?? "",
        Environment.GetEnvironmentVariable("AUTH_RESPONSES") ?? "",
        Environment.GetEnvironmentVariable("BENEFIT_ENTERTAIMENT_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_CATEGORY_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_PAYMENT_METHOD_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_PAYMENT_VARIANT_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_PHOTOS_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_REVIEW_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("ENTERTAIMENT_WISHLIST_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("PROMO_APPLICATION_RESPONSES") ?? "",
        Environment.GetEnvironmentVariable("BENEFIT_TOUR_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_CATEGORY_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_PAYMENT_METHODS_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_PAYMENT_VARIANTS_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_PHOTOS_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_REVIEWS_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_TAGS_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("TOUR_WISHLIST_RESPONSE_TOPIC") ?? "",
        Environment.GetEnvironmentVariable("USER_SERVICE_ACCOUNTS_RESPONSES") ?? "",
        Environment.GetEnvironmentVariable("USER_SERVICE_PROFILE_RESPONSES") ?? "",
    });
});
thread.Start();
app.UseHttpsRedirection();

    app.UseSwagger();
    app.UseSwaggerUI();

app.MapControllers();
app.UseHttpsRedirection();

app.UseCors();
app.Run();