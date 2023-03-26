using Lazuli.Pages.Auth;

namespace LazuliTest
{
    public class LoginTest
    {
        [Fact]
        public void TestLoginPageRender()
        {
            using var context = new TestContext();
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
        public void TestNavToSignup()
        {
            using var context = new TestContext();
            var component = context.RenderComponent<Login>();
            var navManager = context.Services.GetService<FakeNavigationManager>();

            var navbtn = component.Find(".nav-to-signup");

            navbtn.Click();

            // check if after clicking nav-to-signup button, navigation manager navigated to correct site
            Assert.Equal("auth/signup", navManager.ToBaseRelativePath(navManager.Uri));
        }
    }
}
