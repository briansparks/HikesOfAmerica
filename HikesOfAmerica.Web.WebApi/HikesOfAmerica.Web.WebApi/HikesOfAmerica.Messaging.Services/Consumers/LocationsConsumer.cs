using HikesOfAmerica.Core.DataModels;
using HikesOfAmerica.Core.Interfaces;
using HikesOfAmerica.Messaging.Core.Consuming;
using RabbitMQ.Client;
using Serilog;
using System.Threading.Tasks;

namespace HikesOfAmerica.Messaging.Services.Consumers
{
    public class LocationsConsumer : ConsumerBase<LocationSubmission>
    {
        private readonly ISubmissionsManager submissionsManager;

        public LocationsConsumer(IModel argChannel, ISubmissionsManager argSubmissionsManager, ILogger argLogger) : base (argChannel, argLogger)
        {
            submissionsManager = argSubmissionsManager;
        }

        protected async override Task<bool> TryConsume(LocationSubmission locationSubmission)
        {
            await submissionsManager.AddLocationSubmissionAsync(locationSubmission);

            return true;
        }
    }
}
