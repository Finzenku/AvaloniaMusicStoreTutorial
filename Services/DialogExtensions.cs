using HanumanInstitute.MvvmDialogs;
using MusicStore.ViewModels;
using System.ComponentModel;
using System.Threading.Tasks;


namespace MusicStore.Services
{
    public static class DialogExtensions
    {
        public static async Task<AlbumViewModel?> ShowMusicStoreWindowAsync(this IDialogService dialog, INotifyPropertyChanged ownerViewModel)
        {
            var viewModel = dialog.CreateViewModel<MusicStoreViewModel>();
            var result = await dialog.ShowDialogAsync(ownerViewModel, viewModel).ConfigureAwait(true);
            return result == true ? viewModel.SelectedAlbum : null; 
        }
    }
}
