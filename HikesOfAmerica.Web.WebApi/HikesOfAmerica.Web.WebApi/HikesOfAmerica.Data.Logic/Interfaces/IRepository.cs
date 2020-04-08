using HikesOfAmerica.Core.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HikesOfAmerica.Core.Interfaces
{
    public interface IRepository
    {
        Task<List<Location>> GetLocationsAsync();

        Task<Location> GetLocationByNameAsync(string locationName);

        Task<string> AddLocationAsync(Location location);

        Task<string> AddLocationSubmissionAsync(LocationSubmission locationSubmission);

        Task<List<LocationSubmission>> GetLocationSubmissionsAsync();

        Task ApproveLocationRequest(string id);
    }
}
