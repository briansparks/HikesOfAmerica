using HikesOfAmerica.Core.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HikesOfAmerica.Core.Requests
{
    [BindProperties]
    public class LocationRequest
    {
        public string name { get; set; }

        public string description { get; set; }

        public IEnumerable<Trail> trails { get; set; }
        
        [FromForm(Name = "file")] 
        public IFormFile file { get; set; }

        public double longitude { get; set; }

        public double latitude { get; set; }
    }
}
