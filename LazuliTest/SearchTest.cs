using Lazuli.Pages.Search;
using LazuliLibrary.API.Endpoints;
using LazuliLibrary.Authentication;
using LazuliTest.Fakes;
using LazuliTest.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace LazuliTest;

public class SearchTest
{
	[Fact]
	public void TestUserCount()
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

		IRenderedComponent<SearchMain> component = context.RenderComponent<SearchMain>(parameter => parameter
			.Add(p => p.SearchWord, "Bret")
		);

		component.WaitForState(() => component.Instance.LoadingUsers == false);

		// check if the amount of comments in rendered container is correct
		component.WaitForAssertion(() => Assert.Equal(1, component.FindAll(".user-profile").Count),
								   TimeSpan.FromSeconds(10));
	}

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

		IRenderedComponent<SearchMain> component = context.RenderComponent<SearchMain>(parameter => parameter
			.Add(p => p.SearchWord, "rerum")
		);

		component.WaitForState(() => component.Instance.LoadingPosts == false);

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

		IRenderedComponent<SearchMain> component = context.RenderComponent<SearchMain>(parameter => parameter
			.Add(p => p.SearchWord, "quia et suscipit")
		);

		component.WaitForState(() => component.Instance.LoadingPosts == false);

		// check if the amount of posts in rendered container is correct
		component.WaitForAssertion(() => Assert.Equal(1, component.FindAll(".post").Count), TimeSpan.FromSeconds(10));

		component.WaitForElement(".show-comments", TimeSpan.FromSeconds(10));
		component.Find(".show-comments").Click();

		// check if number of comments under 1st post of user 1 is correct
		component.WaitForAssertion(() => Assert.Equal(5, component.FindAll(".comment").Count),
								   TimeSpan.FromSeconds(10));
	}

	[Fact]
	public void TestCommentsCount()
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

		IRenderedComponent<SearchMain> component = context.RenderComponent<SearchMain>(parameter => parameter
			.Add(p => p.SearchWord, "est")
		);

		component.WaitForState(() => component.Instance.LoadingComments == false);

		// check if the amount of comments in rendered container is correct
		component.WaitForAssertion(() => Assert.Equal(12, component.FindAll(".comment").Count),
								   TimeSpan.FromSeconds(10));
	}
}