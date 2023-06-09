﻿using Lazuli.Pages.UserProfile;
using LazuliLibrary.API.Endpoints;
using LazuliLibrary.Authentication;
using LazuliTest.Fakes;
using LazuliTest.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace LazuliTest;

public class UserProfileTest
{
	[Fact]
	public void TestPostCount()
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
		userAuthStateProvider.Login(TestDataHelper.GetFakeAuthUser(1));

		IRenderedComponent<UserProfileMain> component = context.RenderComponent<UserProfileMain>(parameter => parameter
			.Add(p => p.UserId, "1")
		);

		component.WaitForState(() => component.Instance.LoadingPosts == false, TimeSpan.FromSeconds(10));

		// check if the amount of posts in rendered container is correct
		component.WaitForAssertion(() => Assert.Equal(2, component.FindAll(".post").Count), TimeSpan.FromSeconds(10));
	}

	[Fact]
	public void TestPostCommentCount()
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
		userAuthStateProvider.Login(TestDataHelper.GetFakeAuthUser(1));

		IRenderedComponent<UserProfileMain> component = context.RenderComponent<UserProfileMain>(parameter => parameter
			.Add(p => p.UserId, "1")
		);

		component.WaitForState(() => component.Instance.LoadingPosts == false, TimeSpan.FromSeconds(10));

		component.WaitForElement(".show-comments", TimeSpan.FromSeconds(10));
		component.Find(".show-comments").Click();

		// check if number of comments under 1st post of user 1 is correct
		component.WaitForAssertion(() => Assert.Equal(5, component.FindAll(".comment").Count),
								   TimeSpan.FromSeconds(10));
	}

	[Fact]
	public void TestAlbumDisplay()
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
		userAuthStateProvider.Login(TestDataHelper.GetFakeAuthUser(1));

		IRenderedComponent<UserProfileMain> component = context.RenderComponent<UserProfileMain>(parameter => parameter
			.Add(p => p.UserId, "1")
		);

		component.WaitForState(() => component.Instance.LoadingAlbums == false, TimeSpan.FromSeconds(10));

		component.WaitForElement(".display-albums", TimeSpan.FromSeconds(10));
		component.Find(".display-albums").Change(new ChangeEventArgs());

		// check if number of albums in user 1 profile is correct
		component.WaitForAssertion(() => Assert.Equal(1, component.FindAll(".album").Count), TimeSpan.FromSeconds(10));
	}
}