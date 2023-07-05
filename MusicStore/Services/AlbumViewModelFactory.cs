using MusicStore.Interfaces;
using MusicStore.Models;
using MusicStore.ViewModels;
using System.Threading.Tasks;

namespace MusicStore.Services
{
    public class AlbumViewModelFactory : IAlbumViewModelFactory
    {
        private readonly IAlbumCoverService _albumCoverService;

        public AlbumViewModelFactory(IAlbumCoverService albumCoverService)
        {
            _albumCoverService = albumCoverService;
        }

        public async Task<AlbumViewModel> CreateAsync(Album album)
        {
            var viewModel = new AlbumViewModel(album);
            await LoadCoverAsync(viewModel);
            return viewModel;
        }

        private async Task LoadCoverAsync(AlbumViewModel viewModel)
        {
            viewModel.Cover = await _albumCoverService.LoadCoverBitmapAsync(viewModel.Album);
        }
    }
}
