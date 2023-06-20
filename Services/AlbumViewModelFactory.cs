using Microsoft.Extensions.DependencyInjection;
using MusicStore.Models;
using MusicStore.ViewModels;
using System.Threading.Tasks;

namespace MusicStore.Services
{
    public static class AlbumViewModelFactory
    {
        public static async Task<AlbumViewModel> CreateAsync(Album album)
        {
            var viewModel = new AlbumViewModel(album);
            await LoadCoverAsync(viewModel);
            return viewModel;
        }

        private static async Task LoadCoverAsync(AlbumViewModel viewModel)
        {
            var albumCoverService = App.Services.GetService<IAlbumCoverService>();
            if (albumCoverService is not null)
            {
                viewModel.Cover = await albumCoverService.LoadCoverBitmapAsync(viewModel.Album);
            }
        }
    }
}
