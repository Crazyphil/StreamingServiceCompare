using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingServiceCompare.StreamingService.Tidal
{
    class SearchResult<T>
    {
        public int status { get; set; }

        public int limit { get; set; }

        public int offset { get; set; }

        public int totalNumberOfItems { get; set; }

        public List<T> items { get; set; }
    }
}
