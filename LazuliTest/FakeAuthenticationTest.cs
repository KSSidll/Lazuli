using LazuliLibrary.Authentication;
using LazuliTest.Fakes;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace LazuliTest;

public class FakeAuthenticationTest
{
	[Fact]
	public async void TestAuthentication()
	{
		using var context = new TestContext();

		context.Services.AddScoped<AuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

		using IServiceScope scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
		var userAuthStateProvider =
			(IUserAuthenticationStateProvider) scope.ServiceProvider.GetRequiredService<AuthenticationStateProvider>();

		// check if not authenticated by default
		Assert.False(await userAuthStateProvider.IsAuthenticated());

		await userAuthStateProvider.Login(4);

		// check if authenticated after login
		Assert.True(await userAuthStateProvider.IsAuthenticated());

		Assert.Equal(4, await userAuthStateProvider.GetBoundToUserId());

		await userAuthStateProvider.Logout();

		// check if not authenticated after logout
		Assert.False(await userAuthStateProvider.IsAuthenticated());
	}
}