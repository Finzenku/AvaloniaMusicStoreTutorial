using MusicStore.Models;
using MusicStore.ViewModels;
using System.Threading.Tasks;

namespace MusicStore.Services
{
    public interface IAlbumViewModelFactory
    {
        Task<AlbumViewModel> CreateAsync(Album album);
    }
}