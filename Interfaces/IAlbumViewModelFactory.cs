using MusicStore.Models;
using MusicStore.ViewModels;
using System.Threading.Tasks;

namespace MusicStore.Interfaces
{
    public interface IAlbumViewModelFactory
    {
        Task<AlbumViewModel> CreateAsync(Album album);
    }
}