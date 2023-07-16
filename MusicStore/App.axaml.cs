using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using HanumanInstitute.MvvmDialogs;
using HanumanInstitute.MvvmDialogs.Avalonia;
using Microsoft.Extensions.DependencyInjection;
using MusicStore.Interfaces;
using MusicStore.Services;
using MusicStore.ViewModels;
using MusicStore.Views;
using System;

namespace MusicStore
{
    public partial class App : Application
    {
        private readonly IServiceProvider _services = ConfigureServices();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);
                desktop.MainWindow = new MainWindow()
                {
                    DataContext = _services.GetRequiredService<MainWindowViewModel>()
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Services
            services.AddSingleton<IDialogService>(serviceProvider => new DialogService(
                new DialogManager
                (
                    viewLocator: new ViewLocator(), 
                    dialogFactory: new DialogFactory()), 
                    viewModelFactory: viewModelType => serviceProvider.GetService(viewModelType)
                ));
            services.AddSingleton<IAlbumSearchService, iTunesAlbumSearchService>();
            services.AddSingleton<IAlbumDataService, JsonAlbumDataService>();
            services.AddTransient<IAlbumCoverService, HttpAlbumCoverService>();
            services.AddSingleton<IAlbumViewModelFactory, AlbumViewModelFactory>();
            services.AddTransient<IViewDialog<AlbumViewModel>, MusicStoreViewDialog>();

            // HttpClients
            services.AddHttpClient("itunes"); // IHttpClientFactory

            // ViewModels
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<MusicStoreViewModel>();

            return services.BuildServiceProvider();
        }
    }
}