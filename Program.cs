using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StreamingServiceCompare.StreamingService;
using static StreamingServiceCompare.MusicFinder;
using System.IO;

namespace StreamingServiceCompare
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.Title = "Streaming Service Compare";
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();

            if (args.Length != 1)
            {
                Console.Error.WriteLine("Usage: StreamingServiceCompare.exe <MusicDirectory>");
                Console.Error.WriteLine("\tMusicDirectory: The on-disk directory in which to search for music files");
                return -1;
            }

            LocalMusicManager musicManager;
            try
            {
                musicManager = new LocalMusicManager(args[0]);
            } catch (DirectoryNotFoundException)
            {
                Console.Error.WriteLine("Directory {0} not found.", args[0]);
                return 1;
            }

            MusicFinder finder = new MusicFinder();
            StreamWriter sw;
            try
            {
                sw = new StreamWriter(Path.Combine(args[0], "streaming-services.csv"));
                sw.AutoFlush = true;
                sw.WriteLine("Artist;Album;Song;Service");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Could not open results file in music directory: {0}", e.Message);
                return 2;
            }

            int processedArtists = 0;
            finder.OnArtistProcessed += (service, artist, result) =>
            {
                //Console.Write("#");
                processedArtists++;
                PrintProgress(service.Name, processedArtists, musicManager.GetArtists().Count);
                if (!result)
                {
                    //Console.WriteLine("Has no artist {0}", artist);
                    lock (sw)
                    {
                        sw.WriteLine("{1};;;{0}", service.Name, CSV.Escape(artist));
                    }
                }
            };
            finder.OnAlbumProcessed += (service, artist, album, result) =>
            {
                Console.Write(",");
                if (!result)
                {
                    //Console.WriteLine("Has no album {1} for artist {0}", artist, album);
                    lock (sw)
                    {
                        sw.WriteLine("{1};{2};;{0}", service.Name, CSV.Escape(artist), CSV.Escape(album));
                    }
                }
            };
            finder.OnSongProcessed += (service, artist, song, result) =>
            {
                Console.Write(".");
                if (!result)
                {
                    //Console.WriteLine("Has no song {1} for artist {0}", artist, song);
                    lock (sw)
                    {
                        sw.WriteLine("{1};;{2};{0}", service.Name, CSV.Escape(artist), CSV.Escape(song));
                    }
                }
            };

            Dictionary<string, SearchResults> results = new Dictionary<string, SearchResults>(AbstractStreamingService.GetServices().Count);
            foreach (var service in AbstractStreamingService.GetServices())
            {
                processedArtists = 0;
                PrintProgress(service.Name, 0, musicManager.GetArtists().Count);

                SearchResults result = finder.Search(musicManager, service);
                Console.WriteLine();
                results.Add(service.Name, result);
            }
            sw.Close();

            Console.Clear();
            foreach (var result in results)
            {
                Console.WriteLine("{0} has {1}% artists, {2}% songs and {3}% albums from your music collection.", result.Key, result.Value.ArtistPercentage, result.Value.SongPercentage, result.Value.AlbumPercentage);
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to quit.");
            Console.ReadKey(false);
            return 0;
        }

        private static void PrintProgress(string currentService, int currentArtist, int totalArtists)
        {
            Console.Clear();
            Console.WriteLine("Comparing music on {0}...", currentService);
            Console.WriteLine("Processed {0} of {1} artists", currentArtist, totalArtists);
        }
    }
}
