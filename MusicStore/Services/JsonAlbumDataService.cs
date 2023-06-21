using MusicStore.Interfaces;
using MusicStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MusicStore.Services
{
    public class JsonAlbumDataService : IAlbumDataService
    {
        private static string? basePath = Path.GetDirectoryName(AppContext.BaseDirectory);
        private static string filePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(basePath))
                    basePath = "./";
                return Path.Combine(basePath, "albums.json");
            }
        }

        public IList<Album> LoadAlbums()
        {
            List<Album>? albums = null;
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                albums = JsonSerializer.Deserialize<List<Album>>(json);
            }
            return (albums is not null) ? albums : new List<Album>();
        }

        public void SaveAlbums(IList<Album> albums)
        {
            string json = JsonSerializer.Serialize(albums);
            File.WriteAllText(filePath, json);
        }
    }
}
