using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingServiceCompare.StreamingService.Qobuz
{
    class Track : APIItem
    {
        public string performers { get; set; }

        public int duration { get; set; }

        public string title { get; set; }

        public Album album { get; set; }

        public int media_number { get; set; }

        public int track_number { get; set; }

        public bool Matches(string artist, string track)
        {
            // Performers may be null even if the rest of the data is correct. This seems to be a data quality issue at the provider's end.
            return title.Equals(track, StringComparison.OrdinalIgnoreCase) && performers != null && performers.Contains(artist);
        }
    }
}
