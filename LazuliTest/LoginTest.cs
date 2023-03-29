using Lazuli.Data.Database;
using Lazuli.Pages.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Lazuli.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.DataProtection;
using LazuliLibrary.API.Endpoints;

namespace LazuliTest
{
    public class LoginTest
    {
        [Fact]
        public void TestLoginPageRender()
        {
            using var context = new TestContext();

            // TODO somehow mock this (couldn't find anything about how to do that as of yet)
            // creates a database in memory instead of using an actual database
            // might create unexpected behaviour when done in several tests, especially if run asynchronously
            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("TestLoginPageRender")
            );

            context.Services.AddAuthenticationCore();
            context.Services.AddScoped<IDataProtectionProvider, EphemeralDataProtectionProvider>();
            context.Services.AddScoped<ProtectedSessionStorage>();
            context.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();

            var component = context.RenderComponent<Login>();

            // check if the amount of children in rendered container is correct
            Assert.Equal(5, component.Find($".container").ChildElementCount);

            // check if the amount of input forms in rendered container is correct
            Assert.Equal(2, component.FindAll($"input").Count);

            // check if the text on submit button is correct
            Assert.Equal("Log in", component.Find(".submit").TextContent);

            // check if the text on sign up navigation button is correct
            Assert.Equal("Sign up", component.Find(".nav-to-signup").TextContent);
        }

        [Fact]
        public void TestNavToSignupOnNavButtonClick()
        {
            using var context = new TestContext();

            // TODO somehow mock this (couldn't find anything about how to do that as of yet)
            // creates a database in memory instead of using an actual database
            // might create unexpected behaviour when done in several tests, especially if run asynchronously
            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("TestNavToSignupOnNavButtonClick")
            );

            context.Services.AddAuthenticationCore();
            context.Services.AddScoped<IDataProtectionProvider, EphemeralDataProtectionProvider>();
            context.Services.AddScoped<ProtectedSessionStorage>();
            context.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();

            var component = context.RenderComponent<Login>();
            var navManager = context.Services.GetService<FakeNavigationManager>();

            var navbtn = component.Find(".nav-to-signup");

            navbtn.Click();

            // check if after clicking nav-to-signup button, navigation manager
            // navigated to correct site
            Assert.Equal("auth/signup", navManager!.ToBaseRelativePath(navManager.Uri));
        }

        // TODO test correct login scenario, check if authenticated afterwards and that navigation isn't on login page anymore
        // TODO test incorrect login scenarios, nothing should happen

        [Fact]
        public async Task TestCorrectLoginSuccess()
        {
            using var context = new TestContext();

            // TODO somehow mock this (couldn't find anything about how to do that as of yet)
            // creates a database in memory instead of using an actual database
            // might create unexpected behaviour when done in several tests, especially if run asynchronously
            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("TestCorrectLoginSuccess")
            );

            context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
            context.Services.AddAuthenticationCore();
            context.Services.AddScoped<IDataProtectionProvider, EphemeralDataProtectionProvider>();
            context.Services.AddScoped<ProtectedSessionStorage>();
            context.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();

            var component = context.RenderComponent<Signup>();
            var navManager = context.Services.GetService<FakeNavigationManager>();

            // create scope of factory service and use it to create database context and auth state provider
            using var scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var userDbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<UserContext>>();
            var userAuthStateProvider = (UserAuthenticationStateProvider)scope.ServiceProvider.GetRequiredService<AuthenticationStateProvider>();
            var userContext = userDbFactory.CreateDbContext();

            // check if no user in the database
            Assert.Equal(0, userContext.Users?.Count());

            // check if not authenticated
            Assert.False(await userAuthStateProvider.IsAuthenticated());

            string username = "username";
            string password = "passwd";
            int boundToUserId = 3;

            userContext.Add(new User(username, password, boundToUserId));
            userContext.SaveChanges();

            // check if only 1 user was added to database
            Assert.Equal(1, userContext.Users?.Count());

            // set data in login form, then submit
            component.FindAll("input")[0].Change(username);
            component.FindAll("input")[1].Change(password);
            component.Find(".submit").Click();

            // TODO configure bUnit's JSInterop to handle calls below
            //// check if authenticated
            //Assert.True(await userAuthStateProvider.IsAuthenticated());

            // check if page changed
            //Assert.NotEqual("", navManager!.ToBaseRelativePath(navManager.Uri));

            userContext.Dispose();
        }

        [Fact]
        public async Task TestWrongLoginLoginFailure()
        {
            using var context = new TestContext();

            // TODO somehow mock this (couldn't find anything about how to do that as of yet)
            // creates a database in memory instead of using an actual database
            // might create unexpected behaviour when done in several tests, especially if run asynchronously
            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("TestWrongLoginLoginFailure")
            );

            context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
            context.Services.AddAuthenticationCore();
            context.Services.AddScoped<IDataProtectionProvider, EphemeralDataProtectionProvider>();
            context.Services.AddScoped<ProtectedSessionStorage>();
            context.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();

            var component = context.RenderComponent<Signup>();
            var navManager = context.Services.GetService<FakeNavigationManager>();

            // create scope of factory service and use it to create database context and auth state provider
            using var scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var userDbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<UserContext>>();
            var userAuthStateProvider = (UserAuthenticationStateProvider)scope.ServiceProvider.GetRequiredService<AuthenticationStateProvider>();
            var userContext = userDbFactory.CreateDbContext();

            // check if no user in the database
            Assert.Equal(0, userContext.Users?.Count());

            // check if not authenticated
            Assert.False(await userAuthStateProvider.IsAuthenticated());

            string username = "username";
            string password = "passwd";
            int boundToUserId = 3;

            userContext.Add(new User(username, password, boundToUserId));
            userContext.SaveChanges();

            // check if only 1 user was added to database
            Assert.Equal(1, userContext.Users?.Count());

            // set data in login form, then submit
            component.FindAll("input")[0].Change($"{username}a");
            component.FindAll("input")[1].Change(password);
            component.Find(".submit").Click();

            // TODO configure bUnit's JSInterop to handle calls below
            //// check if authenticated
            //Assert.True(await userAuthStateProvider.IsAuthenticated());

            // check if page didn't change
            Assert.Equal("", navManager!.ToBaseRelativePath(navManager.Uri));

            userContext.Dispose();
        }

        [Fact]
        public async Task TestWrongPasswordLoginFailure()
        {
            using var context = new TestContext();

            // TODO somehow mock this (couldn't find anything about how to do that as of yet)
            // creates a database in memory instead of using an actual database
            // might create unexpected behaviour when done in several tests, especially if run asynchronously
            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("TestWrongPasswordLoginFailure")
            );

            context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
            context.Services.AddAuthenticationCore();
            context.Services.AddScoped<IDataProtectionProvider, EphemeralDataProtectionProvider>();
            context.Services.AddScoped<ProtectedSessionStorage>();
            context.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();

            var component = context.RenderComponent<Signup>();
            var navManager = context.Services.GetService<FakeNavigationManager>();

            // create scope of factory service and use it to create database context and auth state provider
            using var scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var userDbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<UserContext>>();
            var userAuthStateProvider = (UserAuthenticationStateProvider)scope.ServiceProvider.GetRequiredService<AuthenticationStateProvider>();
            var userContext = userDbFactory.CreateDbContext();

            // check if no user in the database
            Assert.Equal(0, userContext.Users?.Count());

            // check if not authenticated
            Assert.False(await userAuthStateProvider.IsAuthenticated());

            string username = "username";
            string password = "passwd";
            int boundToUserId = 3;

            userContext.Add(new User(username, password, boundToUserId));
            userContext.SaveChanges();

            // check if only 1 user was added to database
            Assert.Equal(1, userContext.Users?.Count());

            // set data in login form, then submit
            component.FindAll("input")[0].Change(username);
            component.FindAll("input")[1].Change($"{password}a");
            component.Find(".submit").Click();

            // TODO configure bUnit's JSInterop to handle calls below
            //// check if authenticated
            //Assert.True(await userAuthStateProvider.IsAuthenticated());

            // check if page didn't change
            Assert.Equal("", navManager!.ToBaseRelativePath(navManager.Uri));

            userContext.Dispose();
        }

        [Fact]
        public async Task TestNoLoginLoginFailure()
        {
            using var context = new TestContext();

            // TODO somehow mock this (couldn't find anything about how to do that as of yet)
            // creates a database in memory instead of using an actual database
            // might create unexpected behaviour when done in several tests, especially if run asynchronously
            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("TestNoLoginLoginFailure")
            );

            context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
            context.Services.AddAuthenticationCore();
            context.Services.AddScoped<IDataProtectionProvider, EphemeralDataProtectionProvider>();
            context.Services.AddScoped<ProtectedSessionStorage>();
            context.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();

            var component = context.RenderComponent<Signup>();
            var navManager = context.Services.GetService<FakeNavigationManager>();

            // create scope of factory service and use it to create database context and auth state provider
            using var scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var userDbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<UserContext>>();
            var userAuthStateProvider = (UserAuthenticationStateProvider)scope.ServiceProvider.GetRequiredService<AuthenticationStateProvider>();
            var userContext = userDbFactory.CreateDbContext();

            // check if no user in the database
            Assert.Equal(0, userContext.Users?.Count());

            // check if not authenticated
            Assert.False(await userAuthStateProvider.IsAuthenticated());

            string username = "username";
            string password = "passwd";
            int boundToUserId = 3;

            userContext.Add(new User(username, password, boundToUserId));
            userContext.SaveChanges();

            // check if only 1 user was added to database
            Assert.Equal(1, userContext.Users?.Count());

            // set data in login form, then submit
            component.FindAll("input")[0].Change("");
            component.FindAll("input")[1].Change(password);
            component.Find(".submit").Click();

            // TODO configure bUnit's JSInterop to handle calls below
            //// check if authenticated
            //Assert.True(await userAuthStateProvider.IsAuthenticated());

            // check if page didn't change
            Assert.Equal("", navManager!.ToBaseRelativePath(navManager.Uri));

            userContext.Dispose();
        }

        [Fact]
        public async Task TestNoPasswordLoginFailure()
        {
            using var context = new TestContext();

            // TODO somehow mock this (couldn't find anything about how to do that as of yet)
            // creates a database in memory instead of using an actual database
            // might create unexpected behaviour when done in several tests, especially if run asynchronously
            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("TestNoPasswordLoginFailure")
            );

            context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
            context.Services.AddAuthenticationCore();
            context.Services.AddScoped<IDataProtectionProvider, EphemeralDataProtectionProvider>();
            context.Services.AddScoped<ProtectedSessionStorage>();
            context.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();

            var component = context.RenderComponent<Signup>();
            var navManager = context.Services.GetService<FakeNavigationManager>();

            // create scope of factory service and use it to create database context and auth state provider
            using var scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var userDbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<UserContext>>();
            var userAuthStateProvider = (UserAuthenticationStateProvider)scope.ServiceProvider.GetRequiredService<AuthenticationStateProvider>();
            var userContext = userDbFactory.CreateDbContext();

            // check if no user in the database
            Assert.Equal(0, userContext.Users?.Count());

            // check if not authenticated
            Assert.False(await userAuthStateProvider.IsAuthenticated());

            string username = "username";
            string password = "passwd";
            int boundToUserId = 3;

            userContext.Add(new User(username, password, boundToUserId));
            userContext.SaveChanges();

            // check if only 1 user was added to database
            Assert.Equal(1, userContext.Users?.Count());

            // set data in login form, then submit
            component.FindAll("input")[0].Change(username);
            component.FindAll("input")[1].Change("");
            component.Find(".submit").Click();

            // TODO configure bUnit's JSInterop to handle calls below
            //// check if authenticated
            //Assert.True(await userAuthStateProvider.IsAuthenticated());

            // check if page didn't change
            Assert.Equal("", navManager!.ToBaseRelativePath(navManager.Uri));

            userContext.Dispose();
        }
    }
}
