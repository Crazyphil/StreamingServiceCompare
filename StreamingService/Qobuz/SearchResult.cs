using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StreamingServiceCompare.StreamingService.Qobuz
{
    class SearchResult
    {
        public string query { get; set; }

        public TypedResult<Artist> artists { get; set; }

        public TypedResult<Album> albums { get; set; }

        public TypedResult<Track> tracks { get; set; }

        public class TypedResult<T> where T : APIItem
        {
            public int limit { get; set; }

            public int offset { get; set; }

            public int total { get; set; }

            public List<T> items { get; set; }
        }
    }
}
