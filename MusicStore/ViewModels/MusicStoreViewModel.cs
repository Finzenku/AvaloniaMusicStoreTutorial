﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HanumanInstitute.MvvmDialogs;
using MusicStore.Interfaces;
using MusicStore.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace MusicStore.ViewModels
{
    public partial class MusicStoreViewModel : ViewModelBase, IResultDialogViewModel<AlbumViewModel>
    {
        #region Constants

        const int SearchDelayInMilliseconds = 1000;

        #endregion

        #region Private Members

        private readonly IDialogService? _dialogService;
        private readonly IAlbumSearchService? _searchService;
        private readonly IAlbumViewModelFactory? _albumViewModelFactory;
        private readonly System.Timers.Timer _searchTimer;
        private readonly CancellationTokenSource _cts;

        #endregion

        #region Public Properties

        // Determines whether the associated window closed with a result or with nothing
        public bool? DialogResult { get; private set; }
        public AlbumViewModel? DialogResultObject => DialogResult??false ? SelectedAlbum : null;

        public ObservableCollection<AlbumViewModel> SearchResults { get; } = new();


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(BuyMusicCommand))]
        private AlbumViewModel? _selectedAlbum;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SearchAlbumsCommand))]
        private string _searchText = "";

        [ObservableProperty]
        private bool _isBusy;

        #endregion

        #region Public Constructors

        public MusicStoreViewModel()
        {
            _cts = new();
            _searchTimer = new(SearchDelayInMilliseconds);
            _searchTimer.AutoReset = false;
            _searchTimer.Elapsed += TimerElapsed;
            PropertyChanged += OnPropertyChanged;
        }

        public MusicStoreViewModel(IDialogService dialogService, IAlbumSearchService searchService, IAlbumViewModelFactory albumViewModelFactory) : this()
        {
            _dialogService = dialogService;
            _searchService = searchService;
            _albumViewModelFactory = albumViewModelFactory;
        }

        #endregion

        #region Event Methods

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SearchText))
                ResetTimer();
        }

        private async void TimerElapsed(object? sender, ElapsedEventArgs e) => await SearchAlbumsAsync();
        private void ResetTimer()
        {
            _searchTimer.Stop();
            _searchTimer.Interval = SearchDelayInMilliseconds;
            _searchTimer.Start();
        }

        #endregion

        #region Relay Commands

        [RelayCommand(CanExecute = nameof(CanSearchAlbums))]
        private async Task SearchAlbumsAsync(CancellationToken? token = null)
        {
            if (token is null)
                token = _cts.Token;

            // If this method was called via the RelayCommand, we want to make sure to stop the timer so it doesn't trigger again
            _searchTimer.Stop();

            // The timer doesn't know about `CanExecute` so we double check the condition and make sure our search service is available
            if (_searchService is null || !CanSearchAlbums())
                return;

            IsBusy = true;
            SearchResults.Clear();
            await foreach (Album album in _searchService.SearchAsync(SearchText, token))
                SearchResults.Add(await _albumViewModelFactory!.CreateAsync(album));
            IsBusy = false;
        }

        // We can't search if our SearchText is blank and we don't want to start another search while another is still processing
        private bool CanSearchAlbums() => !string.IsNullOrWhiteSpace(SearchText) && !IsBusy &&  _albumViewModelFactory is not null;

        [RelayCommand(CanExecute = nameof(CanBuyMusic))]
        private void BuyMusic()
        {
            DialogResult = true;
            _cts.Cancel();
            _dialogService?.Close(this);
        }
        private bool CanBuyMusic() => SelectedAlbum is not null;

        #endregion
    }
}
