using HikesOfAmerica.Core.Interfaces;
using HikesOfAmerica.Core.Requests.LocationRequest;
using HikesOfAmerica.Data.Persistence.DataModels;
using HikesOfAmerica.Data.Persistence.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HikesOfAmerica.Data.Logic
{
    public class LocationsManager : ILocationsManager
    {
        private IModel channel;
        private IRepository locationsRepository;
        private ILogger<ILocationsManager> logger;

        private const string exchangeName = "Locations";

        public LocationsManager(IModel argChannel, IRepository argLocationsRepository, ILogger<ILocationsManager> argLogger)
        {
            channel = argChannel;
            locationsRepository = argLocationsRepository;
            logger = argLogger;
        }

        public async Task<List<Location>> GetLocationsAsync()
        {
            try
            {
                return await locationsRepository.GetLocationsAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }

            return null;
        }

        public async Task<Location> GetLocationByName(string locationName)
        {
            try
            {
                return await locationsRepository.GetLocationByName(locationName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }

            return null;
        }

        public async Task<string> AddLocation(Location location)
        {
            try
            {
                var result = await locationsRepository.AddLocation(location);

                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }

            return null;
        }

        public bool TrySubmitNewLocation(LocationRequest request)
        {
            try
            {
                var message = JsonConvert.SerializeObject(request);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: exchangeName,
                      routingKey: "",
                      basicProperties: null,
                      body: body);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }

            return false;
        }
    }
}
