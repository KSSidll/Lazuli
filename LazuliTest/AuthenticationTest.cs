using Lazuli.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace LazuliTest
{
    public class AuthenticationTest
    {
        [Fact]
        public async void TestAuthentication()
        {
            using var context = new TestContext();

            context.Services.AddAuthenticationCore();
            context.Services.AddScoped<IDataProtectionProvider, EphemeralDataProtectionProvider>();
            context.Services.AddScoped<ProtectedSessionStorage>();
            context.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();

            // create scope of factory service and use it to create database context
            using var scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var userAuthStateProvider = (UserAuthenticationStateProvider)scope.ServiceProvider.GetRequiredService<AuthenticationStateProvider>();

            // check if not authenticated by default
            Assert.False(await userAuthStateProvider.IsAuthenticated());

            // TODO configure bUnit's JSInterop to handle calls below

            //await userAuthStateProvider.Login(1);

            //// check if authenticated after login
            //Assert.True(await userAuthStateProvider.IsAuthenticated());

            //await userAuthStateProvider.Logout();

            //// check if not authenticated after logout
            //Assert.False(await userAuthStateProvider.IsAuthenticated());   
        }
    }
}
