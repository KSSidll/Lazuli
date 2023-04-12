using Lazuli.Authentication;
using Lazuli.Pages.Posts;
using LazuliLibrary.API.Endpoints;
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
		Assert.Equal(1, component.FindAll(".post").Count);
	}
}