using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace StreamingServiceCompare.StreamingService.Tidal
{
    internal class Album
    {
        public int id { get; set; }

        public string title { get; set; }

        public int duration { get; set; }

        public bool streamReady { get; set; }

        public DateTime streamStartDate { get; set; }

        public bool allowStreaming { get; set; }

        public bool premiumStreamingOnly { get; set; }

        public int numberOfTracks { get; set; }

        public int numberOfVideos { get; set; }

        public int numberOfVolumes { get; set; }

        public DateTime releaseDate { get; set; }

        public string copyright { get; set; }

        public string type { get; set; }

        public string version { get; set; }

        public string url { get; set; }

        public string cover { get; set; }

        [DeserializeAs(Name = "explicit")]
        public bool isExplicit { get; set; }

        public string upc { get; set; }

        public int popularity { get; set; }

        public Artist artist { get; set; }

        public List<Artist> artists { get; set; }

        public bool Matches(string otherArtist, string album)
        {
            return title.Equals(album, StringComparison.OrdinalIgnoreCase) &&
                   (artist.Matches(otherArtist) || artists.Exists(a => a.Matches(otherArtist)));
        }
    }
}
