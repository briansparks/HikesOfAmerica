using HikesOfAmerica.Data.Persistence.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HikesOfAmerica.Core.Requests.LocationRequest
{
    [BindProperties]
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
