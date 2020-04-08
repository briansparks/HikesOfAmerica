using HikesOfAmerica.Core.DataModels;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HikesOfAmerica.Notifications.Clients
{
    public class SendGridEmailClient : IEmailClient
    {
        private ISendGridClient sendGridClient;
        private IEmailClientConfiguration emailClientConfiguration;
        private const string Subject = "New Location Submission Approvals Available";

        public SendGridEmailClient(ISendGridClient argSendGridClient, IEmailClientConfiguration argEmailClientConfiguration)
        {
            sendGridClient = argSendGridClient;
            emailClientConfiguration = argEmailClientConfiguration;
        }

        public async Task SendEmailAsync(List<LocationSubmission> locationRequests)
        {
            var from = new EmailAddress(emailClientConfiguration.FromAddress);
            var to = new EmailAddress(emailClientConfiguration.ToAddress);

            var sb = new StringBuilder();
            sb.AppendLine("New submission available for review:");
            locationRequests.ForEach(x => { sb.AppendLine(x.Name); });

            var msg = MailHelper.CreateSingleEmail(from, to, Subject, null, sb.ToString());
            await sendGridClient.SendEmailAsync(msg);
        }
    }
}
