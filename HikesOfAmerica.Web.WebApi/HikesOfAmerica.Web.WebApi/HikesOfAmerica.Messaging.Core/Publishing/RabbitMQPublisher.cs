using HikesOfAmerica.Core.Interfaces;
using RabbitMQ.Client;

namespace HikesOfAmerica.Core.Publishing
{
    public class RabbitMQPublisher : IPublisher
    {
        private readonly IModel channel;
        private const string exchangeName = "Locations";

        public RabbitMQPublisher(IModel argChannel)
        {
            channel = argChannel;
        }

        public void Publish(byte[] body, string routingKey = "")
        {
            channel.BasicPublish(exchange: exchangeName,
            routingKey: routingKey,
            basicProperties: null,
            body: body);
        }
    }
}
