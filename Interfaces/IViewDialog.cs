using System.ComponentModel;
using System.Threading.Tasks;

namespace MusicStore.Interfaces
{
    public interface IViewDialog<TResult>
    {
        Task<TResult?> ShowDialogAsync(INotifyPropertyChanged ownerViewModel);
    }
}