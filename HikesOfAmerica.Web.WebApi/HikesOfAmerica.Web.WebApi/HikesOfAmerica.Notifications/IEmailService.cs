using System.Threading.Tasks;

namespace HikesOfAmerica.Notifications
{
    internal interface IEmailService
    {
        Task<bool> TrySendEmailAsync();
    }
}
