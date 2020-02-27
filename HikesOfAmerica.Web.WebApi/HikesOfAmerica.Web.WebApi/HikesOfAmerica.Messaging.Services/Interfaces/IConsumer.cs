namespace HikesOfAmerica.Messaging.Services.Interfaces
{
    public interface IConsumer
    {
        void DeliverMessage(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, byte[] body);
    }
}
