﻿using HikesOfAmerica.Data.Persistence.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.Collections.Generic;

namespace HikesOfAmerica.Core.DataModels
{
    public class Location : IDataModel
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<Trail> Trails { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
        
        public string Description { get; set; }

        public List<string> Images { get; set; }
    }
}
