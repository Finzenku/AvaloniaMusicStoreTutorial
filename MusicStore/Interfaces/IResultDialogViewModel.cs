using HanumanInstitute.MvvmDialogs;

namespace MusicStore.Interfaces
{
    public interface IResultDialogViewModel<TResult> : IModalDialogViewModel
    {
        TResult? DialogResultObject { get; }
    }
}
