using Lazuli.Pages.Auth;
using LazuliLibrary.API.Endpoints;
using LazuliLibrary.Authentication;
using LazuliLibrary.Data.Database;
using LazuliTest.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LazuliTest;

public class LoginTest
{
	[Fact]
	public void TestLoginPageRender()
	{
		using var context = new TestContext();

		// creates a database in memory instead of using an actual database
		// every test needs to have unique memory database name to avoid conflicts
		context.Services.AddDbContextFactory<UserContext>(
			opt => opt.UseInMemoryDatabase("TestLoginPageRender")
		);

		context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
		context.Services.AddSingleton<IUserAuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

		IRenderedComponent<Login> component = context.RenderComponent<Login>();

		// check if the amount of children in rendered container is correct
		component.WaitForAssertion(() => Assert.Equal(5, component.Find(".container").ChildElementCount),
								   TimeSpan.FromSeconds(10));

		// check if the amount of input forms in rendered container is correct
		component.WaitForAssertion(() => Assert.Equal(2, component.FindAll("input").Count), TimeSpan.FromSeconds(10));

		// check if the text on submit button is correct
		component.WaitForElement(".submit", TimeSpan.FromSeconds(10));
		Assert.Equal("Log in", component.Find(".submit").TextContent);

		// check if the text on sign up navigation button is correct
		component.WaitForElement(".nav-to-signup", TimeSpan.FromSeconds(10));
		Assert.Equal("Sign up", component.Find(".nav-to-signup").TextContent);
	}

	[Fact]
	public async Task TestCorrectLoginSuccess()
	{
		using var context = new TestContext();

		// creates a database in memory instead of using an actual database
		// every test needs to have unique memory database name to avoid conflicts
		context.Services.AddDbContextFactory<UserContext>(
			opt => opt.UseInMemoryDatabase("TestCorrectLoginSuccess")
		);

		context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
		context.Services.AddSingleton<IUserAuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

		IRenderedComponent<Login> component = context.RenderComponent<Login>();
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

		userContext.Add(new User(username, password, boundToUserId));
		await userContext.SaveChangesAsync();

		// check if only 1 user was added to database
		Assert.Equal(1, userContext.Users?.Count());

		// set data in login form, then submit
		component.WaitForElement("input", TimeSpan.FromSeconds(10));
		component.WaitForElement(".submit", TimeSpan.FromSeconds(10));
		component.FindAll("input")[0].Change(username);
		component.FindAll("input")[1].Change(password);
		component.Find(".submit").Click();

		// check if authenticated
		// For some reason, this wait for state only checks the initial value
		component.WaitForElement(".spinner-border");
		component.WaitForState(() => component.Instance.LoggingIn == false, TimeSpan.FromSeconds(10));
		Assert.True(await userAuthStateProvider.IsAuthenticated());

		await userContext.DisposeAsync();
	}

	[Fact]
	public async Task TestWrongLoginLoginFailure()
	{
		using var context = new TestContext();

		// creates a database in memory instead of using an actual database
		// every test needs to have unique memory database name to avoid conflicts
		context.Services.AddDbContextFactory<UserContext>(
			opt => opt.UseInMemoryDatabase("TestWrongLoginLoginFailure")
		);

		context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
		context.Services.AddSingleton<IUserAuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

		IRenderedComponent<Login> component = context.RenderComponent<Login>();
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
		const int boundToUserId = 3;

		userContext.Add(new User(username, password, boundToUserId));
		await userContext.SaveChangesAsync();

		// check if not authenticated
		Assert.False(await userAuthStateProvider.IsAuthenticated());

		// check if only 1 user was added to database
		Assert.Equal(1, userContext.Users?.Count());

		// set data in login form, then submit
		component.WaitForElement("input", TimeSpan.FromSeconds(10));
		component.WaitForElement(".submit", TimeSpan.FromSeconds(10));
		component.FindAll("input")[0].Change($"{username}a");
		component.FindAll("input")[1].Change(password);
		component.Find(".submit").Click();

		// check if not authenticated
		Assert.False(await userAuthStateProvider.IsAuthenticated());

		await userContext.DisposeAsync();
	}

	[Fact]
	public async Task TestWrongPasswordLoginFailure()
	{
		using var context = new TestContext();

		// creates a database in memory instead of using an actual database
		// every test needs to have unique memory database name to avoid conflicts
		context.Services.AddDbContextFactory<UserContext>(
			opt => opt.UseInMemoryDatabase("TestWrongPasswordLoginFailure")
		);

		context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
		context.Services.AddSingleton<IUserAuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

		IRenderedComponent<Login> component = context.RenderComponent<Login>();
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
		const int boundToUserId = 3;

		userContext.Add(new User(username, password, boundToUserId));
		await userContext.SaveChangesAsync();

		// check if only 1 user was added to database
		Assert.Equal(1, userContext.Users?.Count());

		// set data in login form, then submit
		component.WaitForElement("input", TimeSpan.FromSeconds(10));
		component.WaitForElement(".submit", TimeSpan.FromSeconds(10));
		component.FindAll("input")[0].Change(username);
		component.FindAll("input")[1].Change($"{password}a");
		component.Find(".submit").Click();

		// check if not authenticated
		Assert.False(await userAuthStateProvider.IsAuthenticated());

		await userContext.DisposeAsync();
	}

	[Fact]
	public async Task TestNoLoginLoginFailure()
	{
		using var context = new TestContext();

		// creates a database in memory instead of using an actual database
		// every test needs to have unique memory database name to avoid conflicts
		context.Services.AddDbContextFactory<UserContext>(
			opt => opt.UseInMemoryDatabase("TestNoLoginLoginFailure")
		);

		context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
		context.Services.AddSingleton<IUserAuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

		IRenderedComponent<Login> component = context.RenderComponent<Login>();
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
		const int boundToUserId = 3;

		userContext.Add(new User(username, password, boundToUserId));
		await userContext.SaveChangesAsync();

		// check if only 1 user was added to database
		Assert.Equal(1, userContext.Users?.Count());

		// set data in login form, then submit
		component.WaitForElement("input", TimeSpan.FromSeconds(10));
		component.WaitForElement(".submit", TimeSpan.FromSeconds(10));
		component.FindAll("input")[0].Change("");
		component.FindAll("input")[1].Change(password);
		component.Find(".submit").Click();

		// check if not authenticated
		Assert.False(await userAuthStateProvider.IsAuthenticated());

		await userContext.DisposeAsync();
	}

	[Fact]
	public async Task TestNoPasswordLoginFailure()
	{
		using var context = new TestContext();

		// creates a database in memory instead of using an actual database
		// every test needs to have unique memory database name to avoid conflicts
		context.Services.AddDbContextFactory<UserContext>(
			opt => opt.UseInMemoryDatabase("TestNoPasswordLoginFailure")
		);

		context.Services.AddTransient<IUserEndpoint, FakeUserEndpoint>();
		context.Services.AddSingleton<IUserAuthenticationStateProvider, FakeUserAuthenticationStateProvider>();

		IRenderedComponent<Login> component = context.RenderComponent<Login>();
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
		const int boundToUserId = 3;

		userContext.Add(new User(username, password, boundToUserId));
		await userContext.SaveChangesAsync();

		// check if only 1 user was added to database
		Assert.Equal(1, userContext.Users?.Count());

		// set data in login form, then submit
		component.WaitForElement("input", TimeSpan.FromSeconds(10));
		component.WaitForElement(".submit", TimeSpan.FromSeconds(10));
		component.FindAll("input")[0].Change(username);
		component.FindAll("input")[1].Change("");
		component.Find(".submit").Click();

		// check if not authenticated
		Assert.False(await userAuthStateProvider.IsAuthenticated());

		await userContext.DisposeAsync();
	}
}