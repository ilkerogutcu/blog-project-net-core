using System;
using System.IO;
using System.Threading.Tasks;
using Blog.Consumer.Consumers.Category;
using Blog.Core.DataAccess.ElasticSearch;
using Blog.Core.Settings;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using IHost = Microsoft.Extensions.Hosting.IHost;

namespace Blog.Consumer
{
    internal class Program
    {
        public Program(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private static async Task Main(string[] args)
        {
            var host = AppStartup();
            var elasticSearchService = host.Services.GetService<IElasticSearch>();
            var categoryAddedConsumerLogger = host.Services.GetService<ILogger<CategoryAddedConsumer>>();
            var categoryDeletedConsumerLogger = host.Services.GetService<ILogger<CategoryDeletedConsumer>>();
            var categoryUpdatedConsumerLogger = host.Services.GetService<ILogger<CategoryUpdatedConsumer>>();


            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host(new Uri(RabbitMqSettings.RabbitMqUri), h =>
                {
                    h.Username(RabbitMqSettings.Username);
                    h.Password(RabbitMqSettings.Password);
                });

                cfg.ReceiveEndpoint("category-added-queue",
                    e =>
                    {
                        e.Consumer(() =>
                            new CategoryAddedConsumer(elasticSearchService, categoryAddedConsumerLogger));
                        e.UseMessageRetry(r => r.Immediate(20));
                    });
                
                cfg.ReceiveEndpoint("category-deleted-queue",
                    e =>
                    {
                        e.Consumer(() =>
                            new CategoryDeletedConsumer(elasticSearchService, categoryDeletedConsumerLogger));
                        e.UseMessageRetry(r => r.Immediate(5));
                    });
                cfg.ReceiveEndpoint("category-updated-queue",
                    e =>
                    {
                        e.Consumer(() =>
                            new CategoryUpdatedConsumer(elasticSearchService, categoryUpdatedConsumerLogger));
                        e.UseMessageRetry(r => r.Immediate(5));
                    });
            });
            await bus.StartAsync();
            Console.WriteLine("Listening for register commands");
            Console.ReadLine();
            await bus.StopAsync();
        }

        public IConfiguration Configuration { get; }

        private static IHost AppStartup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var logFilePath = $"{Directory.GetCurrentDirectory() + "/logs"}/{DateTime.Now:yyyy-MM-dd}.txt";

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .MinimumLevel.Verbose()
                .WriteTo.File(logFilePath,
                    retainedFileCountLimit: 1,
                    fileSizeLimitBytes: 5000000,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}")
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            Log.Logger.Information("Application Starting");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IElasticSearch, ElasticSearchManager>();
                })
                .UseSerilog()
                .Build();

            return host;
        }
    }
}