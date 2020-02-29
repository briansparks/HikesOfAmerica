using HikesOfAmerica.Data.Persistence;
using HikesOfAmerica.Messaging.Services.Consumers;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace HikesOfAmerica.Messaging.Services
{
    public class Program
    {
        private const string database = "MongoDB";
        private const string messagingService = "RabbitMQ";

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var dbConnectionString = configuration.GetConnectionString(database);

            var rabbitMQConfig = configuration.GetSection(messagingService);

            var host = configuration["RabbitMQ:Host"];
            var user = configuration["RabbitMQ:Username"];
            var pwd = configuration["RabbitMQ:Password"];

            ConnectionFactory connectionFactory = new ConnectionFactory
            {
                UserName = user,
                Password = pwd,
                HostName = host
            };

            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: $"{typeof(LocationsConsumer)}Queue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            channel.BasicQos(0, 1, false);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
                .CreateLogger();

            var mongoDbRepository = new MongoDbRepository(dbConnectionString);
            LocationsConsumer locationsConsumer = new LocationsConsumer(channel, mongoDbRepository, null);
            channel.BasicConsume($"{typeof(LocationsConsumer)}Queue", false, locationsConsumer);

            Console.ReadLine();
        }
    }
}
