using HikesOfAmerica.Core;
using HikesOfAmerica.Core.Interfaces;
using HikesOfAmerica.Data.Persistence;
using HikesOfAmerica.Notifications.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid;
using Serilog;
using Serilog.Enrichers;
using Serilog.Events;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HikesOfAmerica.Notifications
{
    public class Program
    {
        private const string database = "MongoDB";

        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var mongoConnection = configuration.GetConnectionString(database);
            var apiKey = configuration["EmailService:ApiKey"];
            var toAddress = configuration["EmailService:ToAddress"];
            var fromAddress = configuration["EmailService:FromAddress"];

            var emailConfig = new EmailClientConfiguration(toAddress, fromAddress);

            var repo = new MongoDbRepository(mongoConnection);

            var sendGridClient = new SendGridEmailClient(new SendGridClient(apiKey), emailConfig);
            var submissionsManager = new SubmissionsManager(repo, null);

            const string loggerTemplate = @"{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u4}]<{ThreadId}> [{SourceContext:l}] {Message:lj}{NewLine}{Exception}";
            var baseDir = "C:/logs/";
            var logfile = Path.Combine(baseDir, "notificationslog.txt");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.With(new ThreadIdEnricher())
                .Enrich.FromLogContext()
                .WriteTo.Console(LogEventLevel.Information, loggerTemplate)
                .WriteTo.File(logfile, LogEventLevel.Information, loggerTemplate,
                    rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10)
                .CreateLogger();

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton<IRepository>(repo)
                .AddSingleton<ISubmissionsManager>(x => { return submissionsManager; })
                .AddSingleton<IEmailClientConfiguration, EmailClientConfiguration>(x => { return emailConfig; })
                .AddSingleton<IEmailClient, SendGridEmailClient>(x => { return sendGridClient; })
                .AddSingleton<IEmailService, EmailService>(x => { return new EmailService(sendGridClient, submissionsManager); })
                .BuildServiceProvider();

            var svc = serviceProvider.GetService<IEmailService>();

            Log.Information("Attmepting to send notifications email.");
            await svc.TrySendEmailAsync();
            Log.Information("Process complete.");
        }
    }
}
