using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.OpenSearch;

namespace OpenApiService.Utils
{
    public static class Logging
    {
      
        static OpenSearchSinkOptions _configureOpenSearchSink(IConfiguration configuration,string environment){
            return new OpenSearchSinkOptions(new Uri(configuration["OpenSearchConfiguration:Uri"]!))
            {
                AutoRegisterTemplate = true,
                IndexFormat =  $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".","-")}-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM-DD}",
                NumberOfReplicas =1,
                NumberOfShards = 1
            };
        }

        public static void configureLogging(){
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json",optional:false,reloadOnChange:true).Build();
            Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails()
                    .WriteTo.Debug()
                    .WriteTo.Console()
                    .WriteTo.OpenSearch(_configureOpenSearchSink(configuration,environment))
                    .Enrich.WithProperty("Environment",environment)
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
        }
    }
    
}