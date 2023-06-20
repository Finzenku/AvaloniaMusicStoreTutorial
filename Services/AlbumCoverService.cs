using MusicStore.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Avalonia.Media.Imaging;

namespace MusicStore.Services
{
    public class AlbumCoverService : IAlbumCoverService
    {
        private static string? BasePath = Path.GetDirectoryName(AppContext.BaseDirectory);
        private static char[] InvalidChars = Path.GetInvalidFileNameChars();
        private static string CachePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(BasePath))
                    BasePath = "./";
                return Path.Combine(BasePath, "Cache");
            }
        }

        private IHttpClientFactory _httpClientFactory;

        public AlbumCoverService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private static void CheckCacheFolder()
        {
            if (!Directory.Exists(CachePath))
                Directory.CreateDirectory(CachePath);
        }

        public async Task<Bitmap> LoadCoverBitmapAsync(Album album, CancellationToken? token = null)
        {
            Stream coverStream;
            CheckCacheFolder();

            string fileName = $"{album.Artist} - {album.AlbumTitle}.bmp";
            fileName = string.Concat(fileName.Split(InvalidChars, StringSplitOptions.RemoveEmptyEntries));
            string bmpPath = Path.Combine(CachePath, fileName);
            if (File.Exists(bmpPath))
                coverStream = File.OpenRead(bmpPath);
            else
            {
                var client = _httpClientFactory.CreateClient("itunes");
                var data = await client.GetByteArrayAsync(album.CoverUrl);
                coverStream = new MemoryStream(data);
                // Write the data to the Cache file
                using (var fs = File.OpenWrite(bmpPath))
                    await fs.WriteAsync(data);
            }
            return new Bitmap(coverStream);
        }
    }
}
