using iTunesSearch.Library;
using MusicStore.Interfaces;
using System.Collections.Generic;
using System.Threading;

namespace MusicStore.Services
{
    public class iTunesAlbumSearchService : IAlbumSearchService
    {
        private readonly iTunesSearchManager _searchManager;

        public iTunesAlbumSearchService()
        {
            _searchManager = new();
        }

        public async IAsyncEnumerable<Models.Album> SearchAsync(string searchTerm, CancellationToken? token = null)
        {
            var query = await _searchManager.GetAlbumsAsync(searchTerm).ConfigureAwait(false);
            foreach(var album in query.Albums)
            {
                yield return new Models.Album { Artist = album.ArtistName, AlbumTitle = album.CollectionName, CoverUrl = album.ArtworkUrl100.Replace("100x100bb", "300x300bb") };
            }
        }
    }
}
