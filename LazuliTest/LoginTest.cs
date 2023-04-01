using Lazuli.Data.Database;
using Lazuli.Pages.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using LazuliLibrary.API.Endpoints;
using LazuliTest.Fakes;
using Lazuli.Authentication;

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
            // every test needs to have unique memory database name to avoid conflicts
            // so this should be changed
            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("TestLoginPageRender")
            );

            context.Services.AddSingleton<AuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

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
            // every test needs to have unique memory database name to avoid conflicts
            // so this should be changed
            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("TestNavToSignupOnNavButtonClick")
            );

            context.Services.AddSingleton<AuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

            var component = context.RenderComponent<Login>();
            var navManager = context.Services.GetService<FakeNavigationManager>();

            var navbtn = component.Find(".nav-to-signup");

            navbtn.Click();

            // check if after clicking nav-to-signup button, navigation manager
            // navigated to correct site
            Assert.Equal("auth/signup", navManager!.ToBaseRelativePath(navManager.Uri));
        }

        [Fact]
        public async Task TestCorrectLoginSuccess()
        {
            using var context = new TestContext();

            // TODO somehow mock this (couldn't find anything about how to do that as of yet)
            // creates a database in memory instead of using an actual database
            // every test needs to have unique memory database name to avoid conflicts
            // so this should be changed
            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("TestCorrectLoginSuccess")
            );

            context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
            context.Services.AddSingleton<AuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

            var component = context.RenderComponent<Login>();
            var navManager = context.Services.GetService<FakeNavigationManager>();

            // create scope of factory service and use it to create database context
            using var scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var userDbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<UserContext>>();
            var userContext = userDbFactory.CreateDbContext();

            var userAuthStateProvider = (IUserAuthenticationStateProvider)context.Services.GetRequiredService<AuthenticationStateProvider>();

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

            // check if authenticated
            Assert.True(await userAuthStateProvider.IsAuthenticated());

            userContext.Dispose();
        }

        [Fact]
        public async Task TestWrongLoginLoginFailure()
        {
            using var context = new TestContext();

            // TODO somehow mock this (couldn't find anything about how to do that as of yet)
            // creates a database in memory instead of using an actual database
            // every test needs to have unique memory database name to avoid conflicts
            // so this should be changed
            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("TestWrongLoginLoginFailure")
            );

            context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
            context.Services.AddSingleton<AuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

            var component = context.RenderComponent<Login>();
            var navManager = context.Services.GetService<FakeNavigationManager>();

            // create scope of factory service and use it to create database context
            using var scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var userDbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<UserContext>>();
            var userContext = userDbFactory.CreateDbContext();

            var userAuthStateProvider = (IUserAuthenticationStateProvider)context.Services.GetRequiredService<AuthenticationStateProvider>();

            // check if no user in the database
            Assert.Equal(0, userContext.Users?.Count());

            // check if not authenticated
            Assert.False(await userAuthStateProvider.IsAuthenticated());

            string username = "username";
            string password = "passwd";
            int boundToUserId = 3;

            userContext.Add(new User(username, password, boundToUserId));
            userContext.SaveChanges();

            // check if not authenticated
            Assert.False(await userAuthStateProvider.IsAuthenticated());

            // check if only 1 user was added to database
            Assert.Equal(1, userContext.Users?.Count());

            // set data in login form, then submit
            component.FindAll("input")[0].Change($"{username}a");
            component.FindAll("input")[1].Change(password);
            component.Find(".submit").Click();

            // check if not authenticated
            Assert.False(await userAuthStateProvider.IsAuthenticated());

            userContext.Dispose();
        }

        [Fact]
        public async Task TestWrongPasswordLoginFailure()
        {
            using var context = new TestContext();

            // TODO somehow mock this (couldn't find anything about how to do that as of yet)
            // creates a database in memory instead of using an actual database
            // every test needs to have unique memory database name to avoid conflicts
            // so this should be changed
            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("TestWrongPasswordLoginFailure")
            );

            context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
            context.Services.AddSingleton<AuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

            var component = context.RenderComponent<Login>();
            var navManager = context.Services.GetService<FakeNavigationManager>();

            // create scope of factory service and use it to create database context
            using var scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var userDbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<UserContext>>();
            var userContext = userDbFactory.CreateDbContext();

            var userAuthStateProvider = (IUserAuthenticationStateProvider)context.Services.GetRequiredService<AuthenticationStateProvider>();

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

            // check if not authenticated
            Assert.False(await userAuthStateProvider.IsAuthenticated());

            userContext.Dispose();
        }

        [Fact]
        public async Task TestNoLoginLoginFailure()
        {
            using var context = new TestContext();

            // TODO somehow mock this (couldn't find anything about how to do that as of yet)
            // creates a database in memory instead of using an actual database
            // every test needs to have unique memory database name to avoid conflicts
            // so this should be changed
            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("TestNoLoginLoginFailure")
            );

            context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
            context.Services.AddSingleton<AuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

            var component = context.RenderComponent<Login>();
            var navManager = context.Services.GetService<FakeNavigationManager>();

            // create scope of factory service and use it to create database context
            using var scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var userDbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<UserContext>>();
            var userContext = userDbFactory.CreateDbContext();

            var userAuthStateProvider = (IUserAuthenticationStateProvider)context.Services.GetRequiredService<AuthenticationStateProvider>();

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

            // check if not authenticated
            Assert.False(await userAuthStateProvider.IsAuthenticated());

            userContext.Dispose();
        }

        [Fact]
        public async Task TestNoPasswordLoginFailure()
        {
            using var context = new TestContext();

            // TODO somehow mock this (couldn't find anything about how to do that as of yet)
            // creates a database in memory instead of using an actual database
            // every test needs to have unique memory database name to avoid conflicts
            // so this should be changed
            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("TestNoPasswordLoginFailure")
            );

            context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
            context.Services.AddSingleton<AuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

            var component = context.RenderComponent<Login>();
            var navManager = context.Services.GetService<FakeNavigationManager>();

            // create scope of factory service and use it to create database context
            using var scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var userDbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<UserContext>>();
            var userContext = userDbFactory.CreateDbContext();

            var userAuthStateProvider = (IUserAuthenticationStateProvider)context.Services.GetRequiredService<AuthenticationStateProvider>();

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

            // check if not authenticated
            Assert.False(await userAuthStateProvider.IsAuthenticated());

            userContext.Dispose();
        }
    }
}
