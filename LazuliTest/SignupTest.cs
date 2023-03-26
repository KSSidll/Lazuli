using Lazuli.Pages.Auth;

namespace LazuliTest
{
    public class SignupTest
    {
        [Fact]
        public void TestSignupPageRender()
        {
            using var context = new TestContext();
            var component = context.RenderComponent<Signup>();

            // check if the amount of children in rendered container is correct
            Assert.Equal(5, component.Find($".container").ChildElementCount);

            // check if the amount of input forms in rendered container is correct
            Assert.Equal(2, component.FindAll($"input").Count);

            // check if the text on submit button is correct
            Assert.Equal("Sign up", component.Find(".submit").TextContent);

            // check if the text on sign up navigation button is correct
            Assert.Equal("Log in", component.Find(".nav-to-login").TextContent);
        }

        [Fact]
        public void TestNavToLogin()
        {
            using var context = new TestContext();
            var component = context.RenderComponent<Signup>();
            var navManager = context.Services.GetService<FakeNavigationManager>();

            var navbtn = component.Find(".nav-to-login");

            navbtn.Click();

            // check if after clicking nav-to-login button, navigation manager navigated to correct site
            Assert.Equal("auth/login", navManager.ToBaseRelativePath(navManager.Uri));
        }
    }
}
