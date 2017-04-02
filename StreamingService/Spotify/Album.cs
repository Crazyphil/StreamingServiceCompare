using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace StreamingServiceCompare.StreamingService.Spotify
{
    class Album : APIItem
    {
        public List<Artist> artists { get; set; }

        public string album_type { get; set; }

        public List<string> available_markets { get; set; }

        public bool Matches(string artist, string album)
        {
            return name.Equals(album, StringComparison.OrdinalIgnoreCase) && artists.Exists(a => a.Matches(artist));
        }
    }
}
