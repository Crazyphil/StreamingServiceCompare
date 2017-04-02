using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace StreamingServiceCompare.StreamingService.Tidal
{
    class Artist
    {
        public int id { get; set; }

        public string artist { get; set; }

        public string name
        {
            get { return artist; }
            set { artist = value; }
        }

        public string url { get; set; }

        public string picture { get; set; }

        public int popularity { get; set; }

        public string type { get; set; }

        public bool Matches(string otherArtist)
        {
            return name.Equals(otherArtist, StringComparison.OrdinalIgnoreCase);
        }
    }
}
