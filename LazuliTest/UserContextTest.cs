using LazuliLibrary.Data.Database;
using LazuliLibrary.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LazuliTest;

public class UserContextTest
{
	[Fact]
	public void TestGetUser()
	{
		using var context = new TestContext();

		// creates a database in memory instead of using an actual database
		// every test needs to have unique memory database name to avoid conflicts
		context.Services.AddDbContextFactory<UserContext>(
			opt => opt.UseInMemoryDatabase("TestGetUser")
		);

		// create scope of factory service and use it to create database context
		using IServiceScope scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
		var userDbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<UserContext>>();
		UserContext userContext = userDbFactory.CreateDbContext();

		// check no user in the database
		Assert.Equal(0, userContext.Users?.Count());

		const string username = "username";
		const string password = "passwd";
		const int boundToUserId = 3;

		userContext.Add(new User(username, password, boundToUserId));
		userContext.SaveChanges();

		// check if only 1 user was added to database
		Assert.Equal(1, userContext.Users?.Count());

		var hashedPassword = CipherUtility.Encrypt(password, username);

		// check if added user has expected data
		Assert.True(userContext.Users?.Any(user => user.Login == username && user.Password == hashedPassword &&
												   user.BoundToUserId == boundToUserId));

		// get user that was added
		User? addedUser = userContext.GetUser(username, password);

		// get user that wasn't added
		User? notAddedUser = userContext.GetUser($"{username}NOT!", $"{password}NOT!");

		// check if received user that was added has expected data
		Assert.Equal(username, addedUser!.Login);
		Assert.Equal(hashedPassword, addedUser.Password);
		Assert.Equal(boundToUserId, addedUser.BoundToUserId);

		// check if received user that wasn't added has null value
		Assert.Null(notAddedUser);

		userContext.Dispose();
	}
}