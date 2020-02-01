using HikesOfAmerica.Data.Persistence.DataModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace HikesOfAmerica.Core.Requests.LocationRequest
{
    public class LocationRequest
    {

        public string name { get; set; }

        public string description { get; set; }

        public IEnumerable<Trail> trails { get; set; }

        public IFormFile file { get; set; }

        public double longitude { get; set; }

        public double latitude { get; set; }
    }
}
