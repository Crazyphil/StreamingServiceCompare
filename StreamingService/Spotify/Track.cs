using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace StreamingServiceCompare.StreamingService.Spotify
{
    class Track : APIItem
    {
        public Album album { get; set; }

        public List<Artist> artists { get; set; }

        public List<string> available_markets { get; set; }

        public int disc_number { get; set; }

        public int track_number { get; set; }

        public int duration_ms { get; set; }

        [DeserializeAs(Name = "explicit")]
        public bool is_explicit { get; set; }

        public string preview_url { get; set; }

        public bool Matches(string artist, string track)
        {
            return name.Equals(track, StringComparison.OrdinalIgnoreCase) && artists.Exists(a => a.Matches(artist));
        }
    }
}
