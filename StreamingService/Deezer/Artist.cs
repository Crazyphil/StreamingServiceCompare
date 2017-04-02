using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingServiceCompare.StreamingService.Deezer
{
    class Artist : APIItem
    {
        public string name { get; set; }

        public string picture { get; set; }

        public string picture_small { get; set; }

        public string picture_medium { get; set; }

        public string picture_big { get; set; }

        public string picture_xl { get; set; }

        public int nb_album { get; set; }

        public int nb_fan { get; set; }

        public bool radio { get; set; }

        public string tracklist { get; set; }

        public bool Matches(string artist)
        {
            return name.Equals(artist, StringComparison.OrdinalIgnoreCase) || artist.Contains(name);
        }
    }
}
