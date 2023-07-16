# Avalonia Music Store App

An amateur's take on the [AvaloniaUI Music Store App Tutorial](https://docs.avaloniaui.net/docs/next/tutorials/music-store-app/) project 
using [Microsoft's CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/) framework.   

## CommunityToolkit

I wanted to use the community toolkit to become more familiar with its concepts 
since it now comes as an MVVM framework option for new [Avalonia projects](https://github.com/AvaloniaUI/avalonia-dotnet-templates).  
It cuts out a lot of the boilerplate code that the tutorial uses, 
but libraries like [ReactiveUI.Fody](https://github.com/kswoll/ReactiveUI.Fody) can help with that too.  

I have used [Avalonia with ReactiveUI](https://docs.avaloniaui.net/docs/next/concepts/reactiveui/) in the past but never to an advanced level.


## Inversion of Control

I separated out a lot of business logic away from the `Album` model, put them into services, and then injected them using 
[Microsoft.Extensions.DependencyInjection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection). 
Most of the services just implement a single method but could be expanded upon.

I'm honestly not sure if I set up the `IServiceProvider` in the right place but it seems to do the job.

### Services

#### `IAlbumSearchService`

Dedicated to finding available albums and returning a list of them.  
Implemented with `iTunesAlbumSearchService` to use the iTunes API just like in the original tutorial.

#### `IAlbumCoverService`

Dedicated to getting the cover image for a given `Album`.  
Implemented with `HttpAlbumCoverService` to use an `HttpClient` to download images from the `Album.CoverUrl` 
if it does not find one stored in a local "Cache" folder.

#### `IAlbumDataService`

Dedicated to saving and loading lists of `Album`.  
Implemented with `JsonAlbumDataService` to serialize and deserialize lists of `Album` and save/load them to a local file.


#### `IAlbumViewModelFactory`

One issue I came across is that because the `Album` models are now dumb and don't store the Image for the cover themselves, 
I needed a way to get the `Album` and cover `Image` to the `AlbumViewModel` at the same time 
without giving it access directly to the DI container.
I solved this by creating a `IAlbumViewModelFactory` service that uses the current `IAlbumCoverService` from the DI container.

## Unit Testing

This was my first project that properly implemented Depedency Injection so I ended up using it to learn about Unit Testing too.  
I used [Xunit](https://xunit.net/), which comes built-in to Visual Stuido; 
[FluentAssertions](https://fluentassertions.com/), which lets your write "fluent" (readable) assertions; 
and [FakeItEasy](https://fakeiteasy.github.io/), to fake implementations for my services.

Adding unit tests really made me have to rethink how I approached my services and how they were used, 
mostly because I could not unit test static classes or extention methods.

## Summary

I had a lot of fun doing this tutorial project. I had to think a lot about how to do things in the community toolkit that were being done with ReactiveUI in the tutorial. 
I think it worked out really well with the inclusion of 
[HanumanInstitute.MvvmDialogs.Avalonia](https://github.com/mysteryx93/HanumanInstitute.MvvmDialogs/) 
to handle the MusicStoreView dialog window. I was originally going to use 
[DialogHost.Avalonia](https://github.com/AvaloniaUtils/DialogHost.Avalonia) 
with it but because the tutorial opted to open a whole new window, I didn't feel the need to include it.

I feel like I over-did it with all of the services but I also feel like they all cover specific tasks that don't necessarily need to go together. 
