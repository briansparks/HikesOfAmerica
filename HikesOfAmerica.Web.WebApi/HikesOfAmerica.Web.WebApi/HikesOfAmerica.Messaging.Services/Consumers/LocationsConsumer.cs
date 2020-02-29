using HikesOfAmerica.Data.Persistence.DataModels;
using HikesOfAmerica.Data.Persistence.Interfaces;
using HikesOfAmerica.Messaging.Core.Consuming;
using RabbitMQ.Client;
using Serilog;
using System.Threading.Tasks;

namespace HikesOfAmerica.Messaging.Services.Consumers
{
    public class LocationsConsumer : ConsumerBase<Location>
    {
        private readonly IRepository repository;

        public LocationsConsumer(IModel argChannel, IRepository argRepository, ILogger argLogger) : base (argChannel, argLogger)
        {
            repository = argRepository;
        }

        protected async override Task<bool> TryConsume(Location location)
        {
            await repository.AddLocation(location);

            return true;
        }
    }
}
