using MusicStore.Models;
using System.Collections.Generic;

namespace MusicStore.Interfaces
{
    public interface IAlbumDataService
    {
        void SaveAlbums(IList<Album> albums);
        IList<Album> LoadAlbums();
    }
}
