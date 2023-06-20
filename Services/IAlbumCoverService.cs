using Avalonia.Media.Imaging;
using MusicStore.Models;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.Services
{
    public interface IAlbumCoverService
    {
        Task<Bitmap> LoadCoverBitmapAsync(Album album, CancellationToken? token = null);
    }
}