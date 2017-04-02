using System;
using System.Net;
using RestSharp;

namespace StreamingServiceCompare.StreamingService.Tidal
{
    class TidalStreamingService : AbstractStreamingService
    {
        private const int DEFAULT_LIMIT = 1000;
        private const double LIMIT_SCALE = 3/4;

        public TidalStreamingService() : base("https://listen.tidal.com/v1/")
        {
            client.AddDefaultParameter("token", "P5Xbeo5LFvESeDy6");
            client.AddDefaultParameter("countryCode", region.TwoLetterISORegionName.ToUpper());
            client.AddDefaultParameter("limit", 700);
        }

        public override string Name => "Tidal";

        public override bool HasArtist(string artist)
        {
            return HasItem<Artist>(artist, "artists", result => result.items.Exists(item => item.Matches(artist)));
        }

        public override bool HasAlbum(string artist, string album)
        {
            return HasItem<Album>($"{artist} {album}", "albums", result => result.items.Exists(item => item.Matches(artist, album)));
        }

        public override bool HasSong(string artist, string song)
        {
            return HasItem<Track>($"{artist} {song}", "tracks", result => result.items.Exists(item => item.Matches(artist, song)));
        }

        private bool HasItem<T>(string query, string type, Predicate<SearchResult<T>> criteria)
        {
            IRestRequest request = new RestRequest("search/{type}");
            request.AddUrlSegment("type", type);
            request.AddParameter("query", query);
            return HasItem(request, criteria, 0, result => result.limit, result => result.totalNumberOfItems);
        }

        protected override bool ShouldRetryRequest<T>(IRestRequest request, IRestResponse<T> response, Exception exception)
        {
            var typedResponse = (IRestResponse<SearchResult<Object>>)response;
            if (response.StatusCode == HttpStatusCode.InternalServerError || typedResponse.Data.status == 500)
            {
                int limit = (int)request.Parameters.Find(p => p.Name.Equals("limit")).Value;
                if (limit * LIMIT_SCALE >= 1)
                {
                    request.Parameters.RemoveAll(p => p.Name.Equals("limit"));
                    request.AddParameter("limit", (int)(limit * LIMIT_SCALE));
                    return true;
                }
            }
            return false;
        }
    }
}
