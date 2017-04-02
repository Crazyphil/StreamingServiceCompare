using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingServiceCompare.StreamingService.Spotify
{
    abstract class APIItem
    {
        public string id { get; set; }

        public string name { get; set; }

        public string href { get; set; }

        public string uri { get; set; }

        public int popularity { get; set; }

        public string type { get; set; }
    }
}
