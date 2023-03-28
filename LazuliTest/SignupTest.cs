using Lazuli.Data.Database;
using Lazuli.Pages.Auth;
using Lazuli.Service;
using LazuliLibrary.API;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace LazuliTest
{
    public class SignupTest
    {
        [Fact]
        public void TestSignupPageRender()
        {
            using var context = new TestContext();

            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("userdb")
            );

            // set injected services, TODO mock them instead
            context.Services.AddSingleton<UserService>();
            var api = new ApiHelper();
            var userService = context.Services.GetRequiredService<UserService>();
            userService.setApihelper(api);

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

            context.Services.AddDbContextFactory<UserContext>(
                opt => opt.UseInMemoryDatabase("userdb")
            );

            // set injected services, TODO mock them instead
            context.Services.AddSingleton<UserService>();
            var api = new ApiHelper();
            var userService = context.Services.GetRequiredService<UserService>();
            userService.setApihelper(api);

            var component = context.RenderComponent<Signup>();
            var navManager = context.Services.GetService<FakeNavigationManager>();

            var navbtn = component.Find(".nav-to-login");

            navbtn.Click();

            // check if after clicking nav-to-login button, navigation manager navigated
            // to correct site
            Assert.Equal("auth/login", navManager!.ToBaseRelativePath(navManager.Uri));
        }
    }
}
