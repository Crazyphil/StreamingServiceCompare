using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace StreamingServiceCompare.StreamingService.Tidal
{
    internal class Track
    {
        public int id { get; set; }

        public string title { get; set; }

        public int duration { get; set; }

        public float replayGain { get; set; }

        public float peak { get; set; }

        public bool allowStreaming { get; set; }

        public bool streamReady { get; set; }

        public DateTime streamStartDate { get; set; }

        public bool premiumStreamingOnly { get; set; }

        public int trackNumber { get; set; }

        public int volumeNumber { get; set; }

        public string version { get; set; }

        public int popularity { get; set; }

        public string copyright { get; set; }

        public string url { get; set; }

        public string isrc { get; set; }

        public bool editable { get; set; }

        [DeserializeAs(Name = "explicit")]
        public bool isExplicit { get; set; }

        public string audioQuality { get; set; }

        public Artist artist { get; set; }

        public List<Artist> artists { get; set; }

        public Album album { get; set; }

        public bool Matches(string otherArtist, string track)
        {
            return title.Equals(track, StringComparison.OrdinalIgnoreCase) &&
                   (artist.Matches(otherArtist) || artists.Exists(a => a.Matches(otherArtist)));
        }
    }
}
