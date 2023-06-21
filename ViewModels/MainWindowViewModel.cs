using CommunityToolkit.Mvvm.Input;
using MusicStore.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStore.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region Private Members

        private IViewDialog<AlbumViewModel>? _viewDialogService;
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

        public MainWindowViewModel(IViewDialog<AlbumViewModel> viewDialogService, IAlbumDataService albumDataService, IAlbumViewModelFactory albumViewModelFactory) : this()
        {
            _viewDialogService = viewDialogService;
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
            if (_viewDialogService is null)
                return;
            var album = await _viewDialogService.ShowDialogAsync(this);
            if (album is not null)
            {
                Albums.Add(album);
                _albumDataService?.SaveAlbums(Albums.Select(a => a.Album).ToList());
            }
        }

        #endregion
    }
}