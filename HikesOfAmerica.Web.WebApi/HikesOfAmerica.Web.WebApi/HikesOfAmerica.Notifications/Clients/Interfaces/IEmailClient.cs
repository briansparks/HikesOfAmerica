using HikesOfAmerica.Core.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HikesOfAmerica.Notifications.Clients
{
    public interface IEmailClient
    {
        Task SendEmailAsync(List<LocationSubmission> locationSubmissions);
    }
}
