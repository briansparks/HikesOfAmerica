using HikesOfAmerica.Core.DataModels;
using HikesOfAmerica.Core.Interfaces;
using HikesOfAmerica.Core.Requests;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HikesOfAmerica.Core
{
    public class SubmissionsManager : ISubmissionsManager
    {
        private IPublisher publisher;
        private IRepository locationsRepository;

        public SubmissionsManager(IRepository argLocationsRepository, IPublisher argPublisher)
        {
            locationsRepository = argLocationsRepository;
            publisher = argPublisher;
        }

        public bool TrySubmitNewLocation(LocationRequest request)
        {
            var message = JsonConvert.SerializeObject(request);
            var body = Encoding.UTF8.GetBytes(message);

            publisher.Publish(body);

            return true;
        }

        public async Task<List<LocationSubmission>> GetLocationSubmissionsAsync()
        {
            return await locationsRepository.GetLocationSubmissionsAsync();
        }

        public async Task<string> AddLocationSubmissionAsync(LocationSubmission locationSubmission)
        {
            return await locationsRepository.AddLocationSubmissionAsync(locationSubmission);
        }

        public async Task ApproveLocationRequest(string id)
        {
            await locationsRepository.ApproveLocationRequest(id);
        }
    }
}
