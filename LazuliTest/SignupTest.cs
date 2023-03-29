using Lazuli.Data.Database;
using Lazuli.Pages.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using LazuliLibrary.Utils;
using LazuliLibrary.API.Endpoints;
using LazuliTest.TestDataHelper;
using Lazuli.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.DataProtection;

namespace LazuliTest
{
    public class SignupTest
    {
        [Fact]
        public void TestSignupPageRender()
        {
            using var context = new TestContext();

            // TODO somehow mock this (couldn't find anything about how to do that as of yet)
            // creates a database in memory instead of using an actual database
            // might create unexpected behaviour when done in several tests, especially if run asynchronously
            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("TestSignupPageRenderDB")
            );

            context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
            context.Services.AddAuthenticationCore();
            context.Services.AddScoped<IDataProtectionProvider, EphemeralDataProtectionProvider>();
            context.Services.AddScoped<ProtectedSessionStorage>();
            context.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();

            var component = context.RenderComponent<Signup>();

            // check if the amount of children in rendered container is correct
            Assert.Equal(6, component.Find($".container").ChildElementCount);

            // check if the amount of input forms in rendered container is correct
            Assert.Equal(2, component.FindAll($"input").Count);

            // check if select form exists
            Assert.Equal(1, component.FindAll($"select").Count);

            // check if the text on submit button is correct
            Assert.Equal("Sign up", component.Find(".submit").TextContent);

            // check if the text on sign up navigation button is correct
            Assert.Equal("Log in", component.Find(".nav-to-login").TextContent);
        }

        [Fact]
        public void TestNavToLogin()
        {
            using var context = new TestContext();

            // TODO somehow mock this (couldn't find anything about how to do that as of yet)
            // creates a database in memory instead of using an actual database
            // might create unexpected behaviour when done in several tests, especially if run asynchronously
            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("TestNavToLoginDB")
            );

            context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
            context.Services.AddAuthenticationCore();
            context.Services.AddScoped<IDataProtectionProvider, EphemeralDataProtectionProvider>();
            context.Services.AddScoped<ProtectedSessionStorage>();
            context.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();

            var component = context.RenderComponent<Signup>();
            var navManager = context.Services.GetService<FakeNavigationManager>();

            var navbtn = component.Find(".nav-to-login");

            navbtn.Click();

            // check if after clicking nav-to-login button, navigation manager navigated
            // to correct site
            Assert.Equal("auth/login", navManager!.ToBaseRelativePath(navManager.Uri));
        }

        [Fact]
        public void TestSignup()
        {
            using var context = new TestContext();

            // TODO somehow mock this (couldn't find anything about how to do that as of yet)
            // creates a database in memory instead of using an actual database
            // might create unexpected behaviour when done in several tests, especially if run asynchronously
            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("TestSignupDB")
            );

            context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
            context.Services.AddAuthenticationCore();
            context.Services.AddScoped<IDataProtectionProvider, EphemeralDataProtectionProvider>();
            context.Services.AddScoped<ProtectedSessionStorage>();
            context.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();

            var component = context.RenderComponent<Signup>();
            var navManager = context.Services.GetService<FakeNavigationManager>();

            // create scope of factory service and use it to create database context
            using var scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var userDbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<UserContext>>();
            var userContext = userDbFactory.CreateDbContext();

            // check if no user in the database
            Assert.Equal(0, userContext.Users?.Count());

            string username = "username";
            string password = "passwd";
            int boundToUserId = 3;

            // set username and password in signup form, then submit
            component.FindAll("input")[0].Change(username);
            component.FindAll("input")[1].Change(password);
            component.FindAll("select")[0].Change(boundToUserId);
            component.Find("form").Submit();

            // check if only 1 user was added to database
            Assert.Equal(1, userContext.Users?.Count());

            byte[] hashed_password = CipherUtility.Encrypt(password, username);

            // check if added user has expected data
            Assert.True(userContext.Users?.Any(user => user.Login == username && user.Password == hashed_password && user.BoundToUserId == boundToUserId));
            
            userContext.Dispose();
        }
    }
}
