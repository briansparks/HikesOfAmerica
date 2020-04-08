namespace HikesOfAmerica.Notifications.Clients
{
    public interface IEmailClientConfiguration
    {
        string FromAddress { get; }
        string ToAddress { get; }
    }
}
