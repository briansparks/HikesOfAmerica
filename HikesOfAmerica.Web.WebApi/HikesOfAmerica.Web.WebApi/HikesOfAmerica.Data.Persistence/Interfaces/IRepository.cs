using HikesOfAmerica.Data.Persistence.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HikesOfAmerica.Data.Persistence.Interfaces
{
    public interface IRepository
    {
        Task<List<Location>> GetLocationsAsync();

        Task<Location> GetLocationByName(string locationName);

        Task<string> AddLocation(Location location);
    }
}
