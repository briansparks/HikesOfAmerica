namespace HikesOfAmerica.Core.Interfaces
{
    public interface IPublisher
    {
        void Publish(byte[] body, string routingKey = "");
    }
}
