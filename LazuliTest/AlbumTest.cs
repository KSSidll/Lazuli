﻿using Lazuli.Pages.Album;
using LazuliLibrary.API.Endpoints;
using LazuliLibrary.Authentication;
using LazuliLibrary.Models;
using LazuliTest.Fakes;
using LazuliTest.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace LazuliTest;

public class AlbumTest
{
	[Fact]
	public async void TestAlbumDisplay()
	{
		using var context = new TestContext();

		context.Services.AddSingleton<IUserAuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

		context.Services.AddTransient<IAlbumEndpoint, FakeAlbumEndpoint>();
		context.Services.AddTransient<ICommentEndpoint, FakeCommentEndpoint>();
		context.Services.AddTransient<IPhotoEndpoint, FakePhotoEndpoint>();
		context.Services.AddTransient<IPostEndpoint, FakePostEndpoint>();
		context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();

		using IServiceScope scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
		var userAuthStateProvider = scope.ServiceProvider.GetRequiredService<IUserAuthenticationStateProvider>();
		await userAuthStateProvider.Login(TestDataHelper.GetFakeAuthUser(1));

		AlbumModel? album = await new FakeAlbumEndpoint().GetByAlbumId(1);

		IRenderedComponent<AlbumComponent> component = context.RenderComponent<AlbumComponent>(parameter => parameter
			.Add(p => p.Album, album)
		);

		component.WaitForState(() => component.Instance.LoadingPhotos == false, TimeSpan.FromSeconds(10));

		// check if number of photos in album 1 is correct
		component.WaitForAssertion(() => Assert.Equal(1, component.FindAll(".photo").Count), TimeSpan.FromSeconds(10));
	}
}