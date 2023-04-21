using Lazuli.Pages.Posts;
using LazuliLibrary.API.Endpoints;
using LazuliLibrary.Authentication;
using LazuliTest.Fakes;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace LazuliTest;

public class PostsTest
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

		IRenderedComponent<PostsMain> component = context.RenderComponent<PostsMain>();

		component.WaitForState(() => component.Instance.IsLoadingInitial == false);

		// check if the amount of posts in rendered container is correct
		component.WaitForAssertion(() => Assert.Equal(2, component.FindAll(".post").Count), TimeSpan.FromSeconds(10));

		// load more posts
		component.WaitForElement(".load-more-button", TimeSpan.FromSeconds(10));
		component.Find(".load-more-button").Click();

		component.WaitForState(() => component.Instance.LoadingMoreData == false);

		// check if the amount of posts in rendered container is correct after loading more posts
		component.WaitForAssertion(() => Assert.Equal(4, component.FindAll(".post").Count), TimeSpan.FromSeconds(10));
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

		IRenderedComponent<PostsMain> component = context.RenderComponent<PostsMain>();

		component.WaitForState(() => component.Instance.IsLoadingInitial == false);

		component.WaitForElement(".show-comments", TimeSpan.FromSeconds(10));
		component.Find(".show-comments").Click();

		// check if number of comments under 1st post of user 1 is correct
		component.WaitForAssertion(() => Assert.Equal(5, component.FindAll(".comment").Count),
								   TimeSpan.FromSeconds(10));
	}
}