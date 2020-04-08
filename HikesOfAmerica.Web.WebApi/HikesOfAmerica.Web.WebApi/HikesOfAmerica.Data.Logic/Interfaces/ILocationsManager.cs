using HikesOfAmerica.Core.DataModels;
using HikesOfAmerica.Core.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HikesOfAmerica.Core.Interfaces
{
    public interface ILocationsManager
    {
        Task<List<Location>> GetLocationsAsync();
        Task<string> AddLocationAsync(Location location);
    }
}
