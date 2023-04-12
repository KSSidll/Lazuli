using Lazuli.Authentication;
using Lazuli.Pages.UserProfile;
using LazuliLibrary.API.Endpoints;
using LazuliTest.Fakes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace LazuliTest;

public class UserProfileTest
{
	[Fact]
	public void TestPostCount()
	{
		using var context = new TestContext();

		context.Services.AddSingleton<AuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

		context.Services.AddTransient<IAlbumEndpoint, FakeAlbumEndpoint>();
		context.Services.AddTransient<ICommentEndpoint, FakeCommentEndpoint>();
		context.Services.AddTransient<IPhotoEndpoint, FakePhotoEndpoint>();
		context.Services.AddTransient<IPostEndpoint, FakePostEndpoint>();
		context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();

		using IServiceScope scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
		var userAuthStateProvider =
			(IUserAuthenticationStateProvider) scope.ServiceProvider.GetRequiredService<AuthenticationStateProvider>();
		userAuthStateProvider.Login(1);

		IRenderedComponent<UserProfileMain> component = context.RenderComponent<UserProfileMain>(parameter => parameter
			.Add(p => p.UserId, "1")
		);

		component.WaitForState(() => component.Instance.LoadingPosts == false, TimeSpan.FromSeconds(10));

		// check if the amount of posts in rendered container is correct
		Assert.Equal(2, component.FindAll(".post").Count);
	}

	[Fact]
	public void TestPostCommentCount()
	{
		using var context = new TestContext();

		context.Services.AddSingleton<AuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

		context.Services.AddTransient<IAlbumEndpoint, FakeAlbumEndpoint>();
		context.Services.AddTransient<ICommentEndpoint, FakeCommentEndpoint>();
		context.Services.AddTransient<IPhotoEndpoint, FakePhotoEndpoint>();
		context.Services.AddTransient<IPostEndpoint, FakePostEndpoint>();
		context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();

		using IServiceScope scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
		var userAuthStateProvider =
			(IUserAuthenticationStateProvider) scope.ServiceProvider.GetRequiredService<AuthenticationStateProvider>();
		userAuthStateProvider.Login(1);

		IRenderedComponent<UserProfileMain> component = context.RenderComponent<UserProfileMain>(parameter => parameter
			.Add(p => p.UserId, "1")
		);

		component.WaitForState(() => component.Instance.LoadingPosts == false, TimeSpan.FromSeconds(10));

		component.Find(".show-comments").Click();

		// check if number of comments under 1st post of user 1 is correct
		Assert.Equal(5, component.FindAll(".comment").Count);
	}

	[Fact]
	public void TestAlbumDisplay()
	{
		using var context = new TestContext();

		context.Services.AddSingleton<AuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

		context.Services.AddTransient<IAlbumEndpoint, FakeAlbumEndpoint>();
		context.Services.AddTransient<ICommentEndpoint, FakeCommentEndpoint>();
		context.Services.AddTransient<IPhotoEndpoint, FakePhotoEndpoint>();
		context.Services.AddTransient<IPostEndpoint, FakePostEndpoint>();
		context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();

		using IServiceScope scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
		var userAuthStateProvider =
			(IUserAuthenticationStateProvider) scope.ServiceProvider.GetRequiredService<AuthenticationStateProvider>();
		userAuthStateProvider.Login(1);

		IRenderedComponent<UserProfileMain> component = context.RenderComponent<UserProfileMain>(parameter => parameter
			.Add(p => p.UserId, "1")
		);

		component.WaitForState(() => component.Instance.LoadingAlbums == false, TimeSpan.FromSeconds(10));

		component.Find(".display-albums").Change(new ChangeEventArgs());

		// check if number of albums in user 1 profile is correct
		Assert.Equal(1, component.FindAll(".album").Count);
	}
}