using HikesOfAmerica.Core.DataModels;
using HikesOfAmerica.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HikesOfAmerica.Core
{
    public class LocationsManager : ILocationsManager
    {
        private IRepository locationsRepository;

        public LocationsManager(IRepository argLocationsRepository)
        {
            locationsRepository = argLocationsRepository;
        }

        public async Task<List<Location>> GetLocationsAsync()
        {
            return await locationsRepository.GetLocationsAsync();
        }

        public async Task<Location> GetLocationByNameAsync(string locationName)
        {
            return await locationsRepository.GetLocationByNameAsync(locationName);
        }

        public async Task<string> AddLocationAsync(Location location)
        {
            var result = await locationsRepository.AddLocationAsync(location);

            return result;
        }
    }
}
