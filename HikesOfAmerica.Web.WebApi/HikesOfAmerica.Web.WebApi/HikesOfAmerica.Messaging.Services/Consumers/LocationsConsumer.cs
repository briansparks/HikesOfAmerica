using HikesOfAmerica.Data.Persistence.DataModels;
using HikesOfAmerica.Data.Persistence.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace HikesOfAmerica.Messaging.Services.Consumers
{
    public class LocationsConsumer : DefaultBasicConsumer
    {
        private readonly IModel channel;
        private IRepository repository;
        private ILogger<LocationsConsumer> logger;

        internal LocationsConsumer(IModel argChannel, IRepository argRepository, ILogger<LocationsConsumer> argLogger)
        {
            channel = argChannel;
            repository = argRepository;
            logger = argLogger;
        }

        public async override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            logger.LogInformation($"Message consumed: {consumerTag}, redelivered: {redelivered}");

            try
            {
                var location = JsonConvert.DeserializeObject<Location>(Encoding.UTF8.GetString(body, 0, body.Length));

                await repository.AddLocation(location);

                channel.BasicAck(deliveryTag, false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }
    }
}
