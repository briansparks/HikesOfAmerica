using HikesOfAmerica.Core.Interfaces;
using HikesOfAmerica.Notifications.Clients;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;

namespace HikesOfAmerica.Notifications
{
    internal class EmailService : IEmailService
    {
        private IEmailClient emailClient;
        private ISubmissionsManager submissionsManager;

        internal EmailService(IEmailClient argEmailClient, ISubmissionsManager argSubmissionsManager)
        {
            emailClient = argEmailClient;
            submissionsManager = argSubmissionsManager;
        }

        public async Task<bool> TrySendEmailAsync()
        {
            try
            {
                var submissions = await submissionsManager.GetLocationSubmissionsAsync();

                await emailClient.SendEmailAsync(submissions);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "failed to send notification email.");
            }

            return false;
        }
    }
}
