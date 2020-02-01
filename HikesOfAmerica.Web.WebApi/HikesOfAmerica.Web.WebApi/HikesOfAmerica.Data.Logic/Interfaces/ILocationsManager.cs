using HikesOfAmerica.Core.Requests.LocationRequest;
using HikesOfAmerica.Data.Persistence.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HikesOfAmerica.Core.Interfaces
{
    public interface ILocationsManager
    {
        Task<List<Location>> GetLocationsAsync();
        Task<string> AddLocation(Location location);
        bool TrySubmitNewLocation(LocationRequest request);
    }
}
