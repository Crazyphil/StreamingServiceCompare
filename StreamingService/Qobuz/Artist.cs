using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingServiceCompare.StreamingService.Qobuz
{
    class Artist : APIItem
    {
        public string picture { get; set; }

        public int albums_count { get; set; }

        public string name { get; set; }

        public string slug { get; set; }

        public bool Matches(string artist)
        {
            return name != null && name.Equals(artist, StringComparison.OrdinalIgnoreCase);
        }
    }
}
