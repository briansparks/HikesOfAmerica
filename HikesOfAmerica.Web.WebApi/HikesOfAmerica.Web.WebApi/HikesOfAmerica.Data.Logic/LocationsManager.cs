using HikesOfAmerica.Core.Interfaces;
using HikesOfAmerica.Core.Requests.LocationRequest;
using HikesOfAmerica.Data.Persistence.DataModels;
using HikesOfAmerica.Data.Persistence.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HikesOfAmerica.Data.Logic
{
    public class LocationsManager : ILocationsManager
    {
        private IPublisher publisher;
        private IRepository locationsRepository;
        private ILogger<ILocationsManager> logger;

        public LocationsManager(IPublisher argPublisher, IRepository argLocationsRepository, ILogger<ILocationsManager> argLogger)
        {
            publisher = argPublisher;
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

                publisher.Publish(body);

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
