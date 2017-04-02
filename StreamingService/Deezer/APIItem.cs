using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamingServiceCompare.StreamingService.Deezer
{
    abstract class APIItem
    {
        public long id { get; set; }

        public string link { get; set; }

        public string type { get; set; }
    }
}
