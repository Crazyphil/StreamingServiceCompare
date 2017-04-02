using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace StreamingServiceCompare.StreamingService.Spotify
{
    class Artist : APIItem
    {
        public List<string> genres { get; set; }

        public bool Matches(string artist)
        {
            return name.Equals(artist, StringComparison.OrdinalIgnoreCase);
        }
    }
}
