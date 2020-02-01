using System.Collections.Generic;

namespace HikesOfAmerica.Core.WebRequests
{
    public class Location
    {
        public string Name { get; set; }

        public IEnumerable<Trail> Trails { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public List<string> Images { get; set; }
    }

    public class Trail
    {
        public string Name { get; set; }
        public double Distance { get; set; }
    }
}
