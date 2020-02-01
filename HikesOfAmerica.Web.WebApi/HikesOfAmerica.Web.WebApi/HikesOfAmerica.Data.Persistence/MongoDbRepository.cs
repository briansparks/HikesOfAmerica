using HikesOfAmerica.Data.Persistence.DataModels;
using HikesOfAmerica.Data.Persistence.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using System;

namespace HikesOfAmerica.Data.Persistence
{
    public class MongoDbRepository : IRepository
    {
        private const string DatabaseName = "HikesOfAmerica";
        private const string CollectionName = "Locations";

        private readonly MongoClient mongoClient;

        public MongoDbRepository(string connectionString)
        {
            mongoClient = new MongoClient(connectionString);
        }

        public async Task<List<Location>> GetLocationsAsync()
        {
            var db = mongoClient.GetDatabase(DatabaseName).GetCollection<Location>(CollectionName);

            var result = await db.Find(Builders<Location>.Filter.Empty).ToListAsync();

            return result;
        }

        public async Task<Location> GetLocationByName(string locationName)
        {
            var db = mongoClient.GetDatabase(DatabaseName).GetCollection<Location>(CollectionName);

            var result = await db.Find(x => string.Equals(x.Name, locationName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefaultAsync();

            return result;
        }

        public async Task<string> AddLocation(Location location)
        {
            var db = mongoClient.GetDatabase(DatabaseName).GetCollection<Location>(CollectionName);

            await db.InsertOneAsync(location);

            return location.Id;
        }
    }
}
