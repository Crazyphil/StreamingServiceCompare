using StreamingServiceCompare.StreamingService;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace StreamingServiceCompare
{
    class MusicFinder
    {
        public delegate void ArtistProcessedEventHandler(AbstractStreamingService service, string name, bool result);

        public delegate void AlbumProcessedEventHandler(AbstractStreamingService service, string artistName, string albumName, bool result);

        public delegate void SongProcessedEventHandler(AbstractStreamingService service, string artistName, string songName, bool result);

        public event ArtistProcessedEventHandler OnArtistProcessed;

        public event AlbumProcessedEventHandler OnAlbumProcessed;

        public event SongProcessedEventHandler OnSongProcessed;

        public SearchResults Search(LocalMusicManager musicManager, AbstractStreamingService service)
        {
            int foundArtists = 0, foundAlbums = 0, foundSongs = 0;
#if PARALLEL
            Parallel.ForEach(musicManager.GetArtists(), artist =>
#else
            foreach (var artist in musicManager.GetArtists())
#endif
            {
#if PARALLEL
                if (artist == null) return;
#else
                if (artist == null) continue;
#endif

                if (service.HasArtist(artist))
                {
                    Interlocked.Increment(ref foundArtists);
#if PARALLEL
                    Parallel.ForEach(musicManager.GetSongs()[artist], song =>
#else
                    foreach (var song in musicManager.GetSongs()[artist])
#endif
                    {
#if PARALLEL
                        if (song == null) return;
#else
                        if (song == null) continue;
#endif

                        if (service.HasSong(artist, song))
                        {
                            Interlocked.Increment(ref foundSongs);
                            OnSongProcessed?.Invoke(service, artist, song, true);
                        }
                        else
                        {
                            OnSongProcessed?.Invoke(service, artist, song, false);
                        }
                    }
#if PARALLEL
                    );

                    Parallel.ForEach(musicManager.GetAlbums()[artist], album =>
#else
                    foreach (var album in musicManager.GetAlbums()[artist])
#endif
                    {
#if PARALLEL
                        if (album == null) return;
#else
                        if (album == null) continue;
#endif

                        if (service.HasAlbum(artist, album))
                        {
                            Interlocked.Increment(ref foundAlbums);
                            OnAlbumProcessed?.Invoke(service, artist, album, true);
                        }
                        else
                        {
                            OnAlbumProcessed?.Invoke(service, artist, album, false);
                        }
                    }
#if PARALLEL
                    );
#endif
                    OnArtistProcessed?.Invoke(service, artist, true);
                }
                else
                {
                    OnArtistProcessed?.Invoke(service, artist, false);
                }
            }
#if PARALLEL
            );
#endif

            float totalArtists = musicManager.GetArtists().Count;
            float totalAlbums = musicManager.GetAlbums().Values.Sum(set => set.Count);
            float totalSongs = musicManager.GetSongs().Values.Sum(set => set.Count);

            return new SearchResults(foundArtists / totalArtists * 100f, foundAlbums / totalAlbums * 100f, foundSongs / totalSongs * 100f);
        }

        public class SearchResults
        {
            public SearchResults(float artistPercentage, float albumPercentage, float songPercentage)
            {
                ArtistPercentage = artistPercentage;
                AlbumPercentage = albumPercentage;
                SongPercentage = songPercentage;
            }

            public float ArtistPercentage { get; private set; }
            public float AlbumPercentage { get; private set; }

            public float SongPercentage { get; private set; }
        }
    }
}
