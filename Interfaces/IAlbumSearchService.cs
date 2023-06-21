using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.Interfaces
{
    public interface IAlbumSearchService
    {
        public IAsyncEnumerable<Models.Album> SearchAsync(string searchTerm, CancellationToken? token = null);
    }
}
