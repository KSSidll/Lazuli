using Lazuli.Pages.Auth;
using LazuliLibrary.API.Endpoints;
using LazuliLibrary.Authentication;
using LazuliLibrary.Data.Database;
using LazuliLibrary.Utils;
using LazuliTest.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LazuliTest;

public class SignupTest
{
	[Fact]
	public void TestSignupPageRender()
	{
		using var context = new TestContext();

		// creates a database in memory instead of using an actual database
		// every test needs to have unique memory database name to avoid conflicts
		context.Services.AddDbContextFactory<UserContext>(
			opt => opt.UseInMemoryDatabase("TestSignupPageRender")
		);

		context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
		context.Services.AddSingleton<IUserAuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

		IRenderedComponent<Signup> component = context.RenderComponent<Signup>();

		// check if the amount of children in rendered container is correct
		component.WaitForAssertion(() => Assert.Equal(6, component.Find(".container").ChildElementCount),
								   TimeSpan.FromSeconds(10));

		// check if the amount of input forms in rendered container is correct
		component.WaitForAssertion(() => Assert.Equal(2, component.FindAll("input").Count), TimeSpan.FromSeconds(10));

		// check if select form exists
		component.WaitForAssertion(() => Assert.Equal(1, component.FindAll("select").Count), TimeSpan.FromSeconds(10));

		// check if the text on submit button is correct
		component.WaitForAssertion(() => Assert.Equal("Sign up", component.Find(".submit").TextContent),
								   TimeSpan.FromSeconds(10));

		// check if the text on sign up navigation button is correct
		component.WaitForAssertion(() => Assert.Equal("Log in", component.Find(".nav-to-login").TextContent),
								   TimeSpan.FromSeconds(10));
	}

	[Fact]
	public async void TestCorrectSignupSuccess()
	{
		using var context = new TestContext();

		// creates a database in memory instead of using an actual database
		// every test needs to have unique memory database name to avoid conflicts
		context.Services.AddDbContextFactory<UserContext>(
			opt => opt.UseInMemoryDatabase("TestCorrectSignupSuccess")
		);

		context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
		context.Services.AddSingleton<IUserAuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

		IRenderedComponent<Signup> component = context.RenderComponent<Signup>();
		context.Services.GetService<FakeNavigationManager>();

		// create scope of factory service and use it to create database context
		using IServiceScope scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
		var userDbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<UserContext>>();
		UserContext userContext = await userDbFactory.CreateDbContextAsync();

		var userAuthStateProvider = context.Services.GetRequiredService<IUserAuthenticationStateProvider>();


		// check if no user in the database
		Assert.Equal(0, userContext.Users?.Count());

		// check if not authenticated
		Assert.False(await userAuthStateProvider.IsAuthenticated());

		const string username = "username";
		const string password = "passwd";
		const int boundToUserId = 2;

		// set data in signup form, then submit
		component.WaitForElement("input", TimeSpan.FromSeconds(10));
		component.WaitForElement("select", TimeSpan.FromSeconds(10));
		component.WaitForElement(".submit", TimeSpan.FromSeconds(10));
		component.FindAll("input")[0].Change(username);
		component.FindAll("input")[1].Change(password);
		component.FindAll("select")[0].Change(boundToUserId);
		component.Find(".submit").Click();

		// check if only 1 user was added to database
		Assert.Equal(1, userContext.Users?.Count());

		// check if authenticated
		component.WaitForState(() => component.Instance.SigningUp == false);
		Assert.True(await userAuthStateProvider.IsAuthenticated());

		var hashedPassword = CipherUtility.Encrypt(password, username);

		// check if added user has expected data
		Assert.True(userContext.Users?.Any(user => user.Login == username && user.Password == hashedPassword &&
												   user.BoundToUserId == boundToUserId));

		await userContext.DisposeAsync();
	}

	[Fact]
	public async void TestNoUsernameInFormFail()
	{
		using var context = new TestContext();

		// creates a database in memory instead of using an actual database
		// every test needs to have unique memory database name to avoid conflicts
		context.Services.AddDbContextFactory<UserContext>(
			opt => opt.UseInMemoryDatabase("TestNoUsernameInFormFail")
		);

		context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
		context.Services.AddSingleton<IUserAuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

		IRenderedComponent<Signup> component = context.RenderComponent<Signup>();
		context.Services.GetService<FakeNavigationManager>();

		// create scope of factory service and use it to create database context
		using IServiceScope scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
		var userDbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<UserContext>>();
		UserContext userContext = await userDbFactory.CreateDbContextAsync();

		var userAuthStateProvider = context.Services.GetRequiredService<IUserAuthenticationStateProvider>();

		// check if no user in the database
		Assert.Equal(0, userContext.Users?.Count());

		// check if not authenticated
		Assert.False(await userAuthStateProvider.IsAuthenticated());

		const string username = "";
		const string password = "passwd";
		const int boundToUserId = 3;

		// set data in signup form, then submit
		component.WaitForElement("input", TimeSpan.FromSeconds(10));
		component.WaitForElement("select", TimeSpan.FromSeconds(10));
		component.WaitForElement(".submit", TimeSpan.FromSeconds(10));
		component.FindAll("input")[0].Change(username);
		component.FindAll("input")[1].Change(password);
		component.FindAll("select")[0].Change(boundToUserId);
		component.Find(".submit").Click();

		// check if no user in the database
		Assert.Equal(0, userContext.Users?.Count());

		// check if not authenticated
		Assert.False(await userAuthStateProvider.IsAuthenticated());

		await userContext.DisposeAsync();
	}

	[Fact]
	public async void TestNoPasswordInFormFail()
	{
		using var context = new TestContext();

		// creates a database in memory instead of using an actual database
		// every test needs to have unique memory database name to avoid conflicts
		context.Services.AddDbContextFactory<UserContext>(
			opt => opt.UseInMemoryDatabase("TestNoPasswordInFormFail")
		);

		context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
		context.Services.AddSingleton<IUserAuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

		IRenderedComponent<Signup> component = context.RenderComponent<Signup>();
		context.Services.GetService<FakeNavigationManager>();

		// create scope of factory service and use it to create database context
		using IServiceScope scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
		var userDbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<UserContext>>();
		UserContext userContext = await userDbFactory.CreateDbContextAsync();

		var userAuthStateProvider = context.Services.GetRequiredService<IUserAuthenticationStateProvider>();

		// check if no user in the database
		Assert.Equal(0, userContext.Users?.Count());

		// check if not authenticated
		Assert.False(await userAuthStateProvider.IsAuthenticated());

		const string username = "yser";
		const string password = "";
		const int boundToUserId = 3;

		// set data in signup form, then submit
		component.WaitForElement("input", TimeSpan.FromSeconds(10));
		component.WaitForElement("select", TimeSpan.FromSeconds(10));
		component.WaitForElement(".submit", TimeSpan.FromSeconds(10));
		component.FindAll("input")[0].Change(username);
		component.FindAll("input")[1].Change(password);
		component.FindAll("select")[0].Change(boundToUserId);
		component.Find(".submit").Click();

		// check if no user in the database
		Assert.Equal(0, userContext.Users?.Count());

		// check if not authenticated
		Assert.False(await userAuthStateProvider.IsAuthenticated());

		await userContext.DisposeAsync();
	}

	[Fact]
	public async void TestNoSelectedSelectInFormFail()
	{
		using var context = new TestContext();

		// creates a database in memory instead of using an actual database
		// every test needs to have unique memory database name to avoid conflicts
		context.Services.AddDbContextFactory<UserContext>(
			opt => opt.UseInMemoryDatabase("TestNoSelectedSelectInFormFail")
		);

		context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
		context.Services.AddSingleton<IUserAuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

		IRenderedComponent<Signup> component = context.RenderComponent<Signup>();
		context.Services.GetService<FakeNavigationManager>();

		// create scope of factory service and use it to create database context
		using IServiceScope scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
		var userDbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<UserContext>>();
		UserContext userContext = await userDbFactory.CreateDbContextAsync();

		var userAuthStateProvider = context.Services.GetRequiredService<IUserAuthenticationStateProvider>();

		// check if no user in the database
		Assert.Equal(0, userContext.Users?.Count());

		// check if not authenticated
		Assert.False(await userAuthStateProvider.IsAuthenticated());

		const string username = "ysr";
		const string password = "passwd";
		const int boundToUserId = 0; // 0 -> null

		// set data in signup form, then submit
		component.WaitForElement("input", TimeSpan.FromSeconds(10));
		component.WaitForElement("select", TimeSpan.FromSeconds(10));
		component.WaitForElement(".submit", TimeSpan.FromSeconds(10));
		component.FindAll("input")[0].Change(username);
		component.FindAll("input")[1].Change(password);
		component.FindAll("select")[0].Change(boundToUserId);
		component.Find(".submit").Click();

		// check if no user in the database
		Assert.Equal(0, userContext.Users?.Count());

		// check if not authenticated
		Assert.False(await userAuthStateProvider.IsAuthenticated());

		await userContext.DisposeAsync();
	}
}