using HanumanInstitute.MvvmDialogs;
using MusicStore.ViewModels;

namespace MusicStore.Services
{
    internal class MusicStoreViewDialog : BaseViewDialog<AlbumViewModel, MusicStoreViewModel>
    {
        public MusicStoreViewDialog(IDialogService dialogService) : base(dialogService)
        {
        }
    }
}
