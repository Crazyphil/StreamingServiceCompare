using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;

namespace StreamingServiceCompare.StreamingService.Spotify
{
    class SpotifyStreamingService : AbstractStreamingService
    {
        private const int QUERY_LIMIT = 50;

        public SpotifyStreamingService() : base("https://api.spotify.com/v1/")
        {
            client.AddDefaultParameter("market", region.TwoLetterISORegionName.ToUpper());
            client.AddDefaultParameter("limit", QUERY_LIMIT);
        }

        public override string Name => "Spotify";

        public override bool HasArtist(string artist)
        {
            return HasItem($"artist:\"{artist}\"", "artist", result => result.artists.items.Exists(a => a.Matches(artist)), result => result.artists.total);
        }

        public override bool HasAlbum(string artist, string album)
        {
            return HasItem($"artist:\"{artist}\" album:\"{album}\"", "album", result => result.albums.items.Exists(al => al.Matches(artist, album)), result => result.albums.total);
        }

        public override bool HasSong(string artist, string song)
        {
            return HasItem($"artist:\"{artist}\" track:\"{song}\"", "track", result => result.tracks.items.Exists(t => t.Matches(artist, song)), result => result.tracks.total);
        }
        private bool HasItem(string query, string type, Predicate<SearchResult> criteria, Converter<SearchResult, int> total)
        {
            query = query.Replace(",", "");
            IRestRequest request = new RestRequest("search");
            request.AddParameter("q", query);
            request.AddParameter("type", type);
            return HasItem(request, criteria, 0, result => QUERY_LIMIT, total);
        }

        protected override bool ShouldRetryRequest<T>(IRestRequest request, IRestResponse<T> response, Exception exception)
        {
            if ((int) response.StatusCode == 429)
            {
                Task.Delay(TimeSpan.FromSeconds(Double.Parse(response.Headers.First(p => p.Name.Equals("Retry-After")).Value.ToString())).Add(TimeSpan.FromSeconds(1)));
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // Sometimes, Spotify returns HTTP 404 (rate limiting?)
                return true;
            }
            return false;
        }
    }
}
