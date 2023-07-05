using HanumanInstitute.MvvmDialogs;
using MusicStore.Interfaces;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace MusicStore.Services
{
    internal class BaseViewDialog<TResult, TDialogViewModel> : IViewDialog<TResult> where TDialogViewModel : IResultDialogViewModel<TResult>
    {
        readonly IDialogService _dialogService;

        public BaseViewDialog(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public async Task<TResult?> ShowDialogAsync(INotifyPropertyChanged ownerViewModel, CancellationToken? token = null)
        {
            token?.ThrowIfCancellationRequested();
            var viewModel = _dialogService.CreateViewModel<TDialogViewModel>();
            await _dialogService.ShowDialogAsync(ownerViewModel, viewModel).ConfigureAwait(true);
            return viewModel.DialogResultObject;
        }
    }
}
