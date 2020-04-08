namespace HikesOfAmerica.Notifications.Clients
{
    public class EmailClientConfiguration : IEmailClientConfiguration
    {
        public EmailClientConfiguration(string argToAddress, string argFromAddress)
        {
            ToAddress = argToAddress;
            FromAddress = argFromAddress;
        }

        public string ToAddress { get; }
        public string FromAddress { get; }
    }
}
