using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using MusicStore.Models;

namespace MusicStore.ViewModels
{
    public partial class AlbumViewModel : ViewModelBase
    {
        public Album Album { get; init; }        
        public string Title => Album.AlbumTitle;        
        public string Artist => Album.Artist;

        [ObservableProperty]
        private Bitmap? _cover;

        public AlbumViewModel(Album album)
        {
            Album = album;
        }
    }
}
