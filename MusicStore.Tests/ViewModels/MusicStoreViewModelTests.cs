using FakeItEasy;
using FluentAssertions;
using HanumanInstitute.MvvmDialogs;
using MusicStore.Interfaces;
using MusicStore.Models;
using MusicStore.ViewModels;

namespace MusicStore.Tests.ViewModels
{
    public class MusicStoreViewModelTests
    {
        readonly IDialogService dialogService;
        readonly IAlbumSearchService searchService;
        readonly IAlbumViewModelFactory albumViewModelFactory;
        readonly CancellationTokenSource cts;

        readonly MusicStoreViewModel viewModel;


        static readonly Album testAlbum;
        public static IEnumerable<object?[]> GetAlbums()
        {
            yield return new object?[]
            {
                new Album[] { testAlbum, testAlbum }, 2
            };
            yield return new object?[]
            {
                new Album[] { testAlbum }, 1
            };
            yield return new object?[]
            {
                new Album[0], 0
            };
        }

        static MusicStoreViewModelTests()
        {
            testAlbum = new Album { AlbumTitle= "Test Album", Artist = "Test Artist" };
        }


        public MusicStoreViewModelTests()
        {
            dialogService = A.Fake<IDialogService>();
            searchService = A.Fake<IAlbumSearchService>();
            albumViewModelFactory = A.Fake<IAlbumViewModelFactory>();
            cts = new();

            viewModel = new(dialogService, searchService, albumViewModelFactory);
        }

        [Theory]
        [MemberData(nameof(GetAlbums))]
        public async Task MusiceStoreViewModel_SearchAlbumsCommand(Album[] albums, int albumCount)
        {
            // Arrange
            string searchText = "Test Album";
            CancellationToken token = cts.Token;

            viewModel.SearchText = searchText;
            A.CallTo(() => searchService.SearchAsync(searchText, token)).Returns(albums.ToAsyncEnumerable());

            // Act
            await viewModel.SearchAlbumsCommand.ExecuteAsync(token);

            // Assert
            viewModel.SearchResults.Should().HaveCount(albumCount);
        }

        [Fact]
        public void MusiceStoreViewModel_BuyMusicCommand()
        {
            // Arrange
            var selectedAlbum = new AlbumViewModel(testAlbum);
            viewModel.SelectedAlbum = selectedAlbum;

            // Act
            viewModel.BuyMusicCommand.Execute(null);

            // Assert
            viewModel.DialogResult.Should().Be(true);
            viewModel.DialogResultObject.Should().BeEquivalentTo(selectedAlbum);
        }
    }
}
