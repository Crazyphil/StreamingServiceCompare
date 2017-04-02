using System.Collections.Generic;

namespace StreamingServiceCompare.StreamingService.Spotify
{
    class SearchResult
    {
        public TypedResult<Artist> artists { get; set; }
        
        public TypedResult<Album> albums { get; set; }

        public TypedResult<Track> tracks { get; set; }

        public class TypedResult<T> where T : APIItem
        {
            public string href { get; set; }
            
            public List<T> items { get; set; }

            public int limit { get; set; }

            public int offset { get; set; }

            public int total { get; set; }
        }
    }
}
