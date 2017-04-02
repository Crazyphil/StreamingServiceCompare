using System;
using RestSharp;

namespace StreamingServiceCompare.StreamingService.Qobuz
{
    class QobuzStreamingService : AbstractStreamingService
    {
        public QobuzStreamingService() : base("http://www.qobuz.com/api.json/0.2/")
        {
            client.AddDefaultHeader("X-App-Id", "285473059");
            client.AddDefaultParameter("limit", "500");
        }

        public override string Name => "Qobuz";

        public override bool HasArtist(string artist)
        {
            string nArtist = RemoveDiacritics(artist);
            return HasItem(nArtist, "artist", result => result.artists.items.Exists(a => a.Matches(artist) || a.Matches(nArtist)), result => result.artists.total);
        }

        public override bool HasAlbum(string artist, string album)
        {
            string nArtist = RemoveDiacritics(artist);
            string nAlbum = RemoveDiacritics(album);
            return HasItem($"{nArtist} {nAlbum}", "album", result => result.albums.items.Exists(a => a.Matches(artist, album) || a.Matches(nArtist, nAlbum)), result => result.albums.total);
        }

        public override bool HasSong(string artist, string song)
        {
            string nArtist = RemoveDiacritics(artist);
            string nSong = RemoveDiacritics(song);
            return HasItem($"{nArtist} {nSong}", "track", result => result.tracks.items.Exists(t => t.Matches(artist, song) || t.Matches(nArtist, nSong)), result => result.tracks.total);
        }
        
        private bool HasItem(string query, string type, Predicate<SearchResult> criteria, Converter<SearchResult, int> total)
        {
            IRestRequest request = new RestRequest("{type}/search");
            request.AddUrlSegment("type", type);
            request.AddParameter("query", query);
            return HasItem(request, criteria, 0, result => result.artists.limit, total);
        }

        protected override bool ShouldRetryRequest<T>(IRestRequest request, IRestResponse<T> response, Exception exception)
        {
            return false;
        }
    }
}
