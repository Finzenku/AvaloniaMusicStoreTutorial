using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using MusicStore.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region Private Members

        private IDialogService? _dialogService;
        private IAlbumDataService? _albumDataService;
        private IAlbumViewModelFactory? _albumViewModelFactory;

        #endregion

        #region Public Properties

        public ObservableCollection<AlbumViewModel> Albums { get; }

        #endregion

        #region Public Constructors

        public MainWindowViewModel() 
        {
            Albums = new();
        }

        public MainWindowViewModel(IDialogService dialogService, IAlbumDataService albumDataService, IAlbumViewModelFactory albumViewModelFactory) : this()
        {
            _dialogService = dialogService;
            _albumDataService = albumDataService;
            _albumViewModelFactory = albumViewModelFactory;
            _ = LoadAlbumsAsync();
        }

        #endregion

        #region Private Methods

        private async Task LoadAlbumsAsync()
        {
            if (_albumDataService is not null && _albumViewModelFactory is not null)
                foreach (var album in _albumDataService.LoadAlbums())
                    Albums.Add(await _albumViewModelFactory.CreateAsync(album));
        }

        #endregion

        #region Relay Commands

        [RelayCommand]
        private async Task BuyMusicAsync()
        {
            if (_dialogService is null)
                return;
            var album = await _dialogService.ShowMusicStoreWindowAsync(this);
            if (album is not null)
            {
                Albums.Add(album);
                _albumDataService?.SaveAlbums(Albums.Select(a => a.Album).ToList());
            }
        }

        #endregion
    }
}