using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using System;
using HikesOfAmerica.Core.DataModels;
using HikesOfAmerica.Core.Interfaces;
using System.Linq;

namespace HikesOfAmerica.Data.Persistence
{
    public class MongoDbRepository : IRepository
    {
        private const string DatabaseName = "HikesOfAmerica";
        private const string LocationsCollectionName = "Locations";
        private const string SubmissionsCollectionName = "Submissions";

        private readonly MongoClient mongoClient;

        public MongoDbRepository(string connectionString)
        {
            mongoClient = new MongoClient(connectionString);
        }

        public async Task<List<Location>> GetLocationsAsync()
        {
            var db = mongoClient.GetDatabase(DatabaseName).GetCollection<Location>(LocationsCollectionName);

            var result = await db.Find(Builders<Location>.Filter.Empty).ToListAsync();

            return result;
        }

        public async Task<Location> GetLocationByNameAsync(string locationName)
        {
            var db = mongoClient.GetDatabase(DatabaseName).GetCollection<Location>(LocationsCollectionName);

            var result = await db.Find(x => string.Equals(x.Name, locationName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefaultAsync();

            return result;
        }

        public async Task<string> AddLocationAsync(Location location)
        {
            var db = mongoClient.GetDatabase(DatabaseName).GetCollection<Location>(LocationsCollectionName);

            await db.InsertOneAsync(location);

            return location.Id;
        }

        public async Task<string> AddLocationSubmissionAsync(LocationSubmission locationSubmission)
        {
            var db = mongoClient.GetDatabase(DatabaseName).GetCollection<LocationSubmission>(SubmissionsCollectionName);

            await db.InsertOneAsync(locationSubmission);

            return locationSubmission.Id;
        }

        public async Task<List<LocationSubmission>> GetLocationSubmissionsAsync()
        {
            var db = mongoClient.GetDatabase(DatabaseName).GetCollection<LocationSubmission>(SubmissionsCollectionName);

            var result = await db.Find(x => !x.Approved).ToListAsync();

            return result;
        }

        public async Task ApproveLocationRequest(string id)
        {
            var submissionsCollection = mongoClient.GetDatabase(DatabaseName).GetCollection<LocationSubmission>(SubmissionsCollectionName);
            var locationsCollection = mongoClient.GetDatabase(DatabaseName).GetCollection<Location>(LocationsCollectionName);

            var result = await submissionsCollection.Find(x => x.Id == id).ToListAsync();

            if (result.Any())
            {
                var submission = result.First();

                var doc = new Location()
                {
                    Name = submission.Name,
                    Description = submission.Description,
                    Images = submission.Images,
                    Trails = submission.Trails,
                    Latitude = submission.Latitude,
                    Longitude = submission.Longitude
                };

                await locationsCollection.InsertOneAsync(doc);
                // TODO: set submission to approved in db
            }
        }
    }
}
