using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingServiceCompare.StreamingService.Deezer
{
    class Track : APIItem
    {
        public bool readable { get; set; }

        public string title { get; set; }

        public string title_short { get; set; }

        public string title_version { get; set; }

        public int duration { get; set; }

        public int rank { get; set; }

        public bool explicit_lyrics { get; set; }

        public string preview { get; set; }

        public Artist artist { get; set; }

        public Album album { get; set; }

        public bool Matches(string artist, string track)
        {
            return (title.Equals(track, StringComparison.OrdinalIgnoreCase) || title_short != null && title_short.Equals(track, StringComparison.OrdinalIgnoreCase)) && this.artist.Matches(artist);
        }
    }
}
