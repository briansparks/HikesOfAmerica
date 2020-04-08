using RabbitMQ.Client;
using HikesOfAmerica.Data.Persistence.Interfaces;
using Serilog;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace HikesOfAmerica.Messaging.Core.Consuming
{
    public abstract class ConsumerBase<T> : DefaultBasicConsumer
        where T : IDataModel
    {
        private readonly IModel channel;
        private readonly ILogger logger;

        private static int maxRetries = 5;

        public ConsumerBase(IModel argChannel, ILogger argLogger)
        {
            logger = argLogger;
            channel = argChannel;
        }

        public sealed override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            try
            {
                //logger.Information($"Message of type {this.GetType()} received.");

                var obj = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(body, 0, body.Length));
                TryConsume(obj).Wait();
                channel.BasicAck(deliveryTag, false);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }

        protected abstract Task<bool> TryConsume(T obj);
    }
}
