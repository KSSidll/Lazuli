using LazuliTest.Fakes;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace LazuliTest
{
    public class AuthenticationTest
    {
        [Fact]
        public async void TestAuthentication()
        {
            using var context = new TestContext();

            context.Services.AddScoped<AuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

            using var scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var userAuthStateProvider = (FakeUserAuthenticationStateProvider)scope.ServiceProvider.GetRequiredService<AuthenticationStateProvider>();

            // check if not authenticated by default
            Assert.False(await userAuthStateProvider.IsAuthenticated());

            await userAuthStateProvider.Login(4);

            // check if authenticated after login
            Assert.True(await userAuthStateProvider.IsAuthenticated());

            await userAuthStateProvider.Logout();

            // check if not authenticated after logout
            Assert.False(await userAuthStateProvider.IsAuthenticated());
        }
    }
}
