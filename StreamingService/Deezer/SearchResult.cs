using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingServiceCompare.StreamingService.Deezer
{
    abstract class SearchResult
    {
        public int total { get; set; }

        public string prev { get; set; }

        public string next { get; set; }

        public Error error { get; set; }

        public class Error
        {
            public string type { get; set; }

            public string message { get; set; }

            public int code { get; set; }
        }
    }

    class SearchResult<T> : SearchResult where T : APIItem
    {
        public List<T> data { get; set; }
    }
}
