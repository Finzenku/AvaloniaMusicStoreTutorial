using FakeItEasy;
using FluentAssertions;
using MusicStore.Interfaces;
using MusicStore.Models;
using MusicStore.ViewModels;

namespace MusicStore.Tests.ViewModels
{
    public class MainWindowViewModelTests
    {
        readonly IViewDialog<AlbumViewModel> viewDialogService;
        readonly IAlbumDataService dataService;
        readonly IAlbumViewModelFactory albumViewModelFactory;
        static readonly Album testAlbum;

        readonly MainWindowViewModel viewModel;

        public static IEnumerable<object?[]> GetAlbumViewModels()
        {
            yield return new object?[] { new AlbumViewModel(testAlbum) };
            yield return new object?[] { null };
        }

        static MainWindowViewModelTests()
        {
            testAlbum = new Album { AlbumTitle="Test Album", Artist="Test Artist" };
        }

        public MainWindowViewModelTests()
        {
            // Dependencies
            viewDialogService = A.Fake<IViewDialog<AlbumViewModel>>();
            dataService = A.Fake<IAlbumDataService>();
            albumViewModelFactory = A.Fake<IAlbumViewModelFactory>();

            // SUT
            viewModel = new(viewDialogService, dataService, albumViewModelFactory);
        }

        [Theory]
        [MemberData(nameof(GetAlbumViewModels))]
        public async Task MainWindowViewModel_BuyMusicCommand(AlbumViewModel? dialogResult)
        {
            // Arrange
            A.CallTo(() => viewDialogService.ShowDialogAsync(viewModel, null)).Returns(dialogResult);
            int albumCount = viewModel.Albums.Count();

            // Act
            await viewModel.BuyMusicCommand.ExecuteAsync(null);

            // Assert
            viewModel.Albums.Should().AllBeOfType<AlbumViewModel>();
            viewModel.Albums.Should().HaveCountGreaterThanOrEqualTo(albumCount);
        }

        [Fact]
        public void MainWindowViewModel_ResetLibraryCommand()
        {
            // Arrange

            // Act
            viewModel.ResetLibraryCommand.Execute(null);

            // Assert
            viewModel.Albums.Should().NotBeNull();
            viewModel.Albums.Should().BeEmpty();
        }
            
    }
}
