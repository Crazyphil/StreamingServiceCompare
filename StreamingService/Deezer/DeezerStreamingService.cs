using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace StreamingServiceCompare.StreamingService.Deezer
{
    class DeezerStreamingService : AbstractStreamingService
    {
        private const int QUERY_LIMIT = 100;

        public DeezerStreamingService() : base("http://api.deezer.com/")
        {
            client.AddDefaultParameter("limit", QUERY_LIMIT);
        }

        public override string Name => "Deezer";

        protected override string OffsetParameter => "index";

        public override bool HasArtist(string artist)
        {
            return HasItem<Artist>(artist, "artist", data => CheckValidResult(result => result.data.Exists(a => a.Matches(artist)), data));
        }

        public override bool HasAlbum(string artist, string album)
        {
            return HasItem<Album>($"{artist} {album}", "album", data => CheckValidResult(result => result.data.Exists(a => a.Matches(artist, album)), data));
        }

        public override bool HasSong(string artist, string song)
        {
            return HasItem<Track>($"{artist} {song}", "album", data => CheckValidResult(result => result.data.Exists(t => t.Matches(artist, song)), data));
        }

        private bool HasItem<T>(string query, string type, Predicate<SearchResult<T>> criteria) where T : APIItem
        {
            IRestRequest request = new RestRequest("search/{type}");
            request.AddUrlSegment("type", type);
            request.AddParameter("q", query);
            return HasItem(request, criteria, 0, result => QUERY_LIMIT, result => result.total);
        }

        private bool CheckValidResult<T>(Predicate<SearchResult<T>> criteria, SearchResult<T> data) where T : APIItem
        {
            if (data.error == null)
            {
                return criteria(data);
            }
            throw new InvalidResultException(data.error.message);
        }

        protected override bool ShouldRetryRequest<T>(IRestRequest request, IRestResponse<T> response, Exception exception)
        {
            var data = response.Data as SearchResult;
            if (data.error.code == 4)
            {
                // Quota exceeded
                Task.Delay(1000);
                return true;
            }
            return false;
        }
    }
}
