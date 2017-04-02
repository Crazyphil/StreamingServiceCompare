using System;
using System.Collections.Generic;
using System.IO;
using TagLib;

namespace StreamingServiceCompare
{
    class LocalMusicManager
    {
        private DirectoryInfo localPath;

        private MusicCache cache;

        public LocalMusicManager(string localPath)
        {
            this.localPath = new DirectoryInfo(localPath);
            if (!this.localPath.Exists)
            {
                throw new DirectoryNotFoundException();
            }
        }

        public HashSet<String> GetArtists()
        {
            if (cache == null)
            {
                LoadMusic();
            }
            return cache.Artists;
        }

        public Dictionary<string, HashSet<String>> GetAlbums()
        {
            if (cache == null)
            {
                LoadMusic();
            }
            return cache.Albums;
        }

        public Dictionary<string, HashSet<String>> GetSongs()
        {
            if (cache == null)
            {
                LoadMusic();
            }
            return cache.Songs;
        }

        private void LoadMusic()
        {
            cache = new MusicCache();
            cache.Artists = new HashSet<string>();
            cache.Albums = new Dictionary<string, HashSet<string>>();
            cache.Songs = new Dictionary<string, HashSet<string>>();

            LoadMusicFromDirectory(localPath);
        }

        private void LoadMusicFromDirectory(DirectoryInfo dir)
        {
            Console.WriteLine("Crawling music in {0}...", dir.FullName);
            foreach (var file in dir.EnumerateFiles())
            {
                TagLib.File tagFile;
                try
                {
                    tagFile = TagLib.File.Create(file.FullName);
                }
                catch (Exception e) when (e is UnsupportedFormatException || e is CorruptFileException)
                {
                    continue;
                }
                if (tagFile.Properties == null || (tagFile.Properties.MediaTypes & MediaTypes.Audio) == MediaTypes.None)
                {
                    continue;
                }

                string artist = tagFile.Tag.FirstPerformer;
                cache.Artists.Add(artist);

                if (!cache.Albums.ContainsKey(artist))
                {
                    cache.Albums.Add(artist, new HashSet<string>());
                }
                cache.Albums[artist].Add(tagFile.Tag.Album);

                if (!cache.Songs.ContainsKey(artist))
                {
                    cache.Songs.Add(artist, new HashSet<string>());
                }
                cache.Songs[artist].Add(tagFile.Tag.Title);
            }
            foreach (var subdir in dir.EnumerateDirectories())
            {
                LoadMusicFromDirectory(subdir);
            }
        }

        private class MusicCache
        {
            public HashSet<string> Artists { get; set; }

            public Dictionary<string, HashSet<string>> Albums { get; set; }

            public Dictionary<string, HashSet<string>> Songs { get; set; }
        }
    }
}
