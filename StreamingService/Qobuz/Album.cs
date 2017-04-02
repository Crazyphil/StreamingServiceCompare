using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Deserializers;

namespace StreamingServiceCompare.StreamingService.Qobuz
{
    class Album : APIItem
    {
        public int tracks_count { get; set; }

        public string title { get; set; }

        public long released_at { get; set; }

        public int duration { get; set; }

        public int media_count { get; set; }

        public Artist artist { get; set; }

        public long qobuz_id { get; set; }

        public double popularity { get; set; }

        public bool purchasable { get; set; }

        public bool streamable { get; set; }

        public bool previewable { get; set; }

        public bool sampleable { get; set; }

        public bool downloadable { get; set; }

        public bool displayable { get; set; }

        public long purchasable_at { get; set; }

        public long streamable_at { get; set; }

        public float maximum_sampling_rate { get; set; }

        public int maximum_bit_depth { get; set; }

        public bool hires { get; set; }

        public bool Matches(string artist, string album)
        {
            return title.Equals(album, StringComparison.OrdinalIgnoreCase) && this.artist.Matches("Various Artists") || this.artist.Matches(artist);
        }
    }
}
