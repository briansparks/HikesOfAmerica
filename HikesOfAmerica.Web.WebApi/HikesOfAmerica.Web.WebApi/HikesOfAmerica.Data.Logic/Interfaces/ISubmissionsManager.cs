using HikesOfAmerica.Core.DataModels;
using HikesOfAmerica.Core.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HikesOfAmerica.Core.Interfaces
{
    public interface ISubmissionsManager
    {
        Task<List<LocationSubmission>> GetLocationSubmissionsAsync();
        Task<string> AddLocationSubmissionAsync(LocationSubmission locationSubmission);
        bool TrySubmitNewLocation(LocationRequest request);
        Task ApproveLocationRequest(string id);
    }
}
