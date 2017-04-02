using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingServiceCompare.StreamingService.Deezer
{
    class Album : APIItem
    {
        public string title { get; set; }

        public string cover { get; set; }

        public string cover_small { get; set; }

        public string cover_medium { get; set; }

        public string cover_big { get; set; }

        public string cover_xl { get; set; }

        public int genre_id { get; set; }

        public int nb_tracks { get; set; }

        public string record_type { get; set; }

        public string tracklist { get; set; }

        public bool explicit_lyrics { get; set; }

        public Artist artist { get; set; }

        public bool Matches(string artist, string album)
        {
            return title.Equals(album, StringComparison.OrdinalIgnoreCase) && this.artist.Matches(artist);
        }
    }
}
