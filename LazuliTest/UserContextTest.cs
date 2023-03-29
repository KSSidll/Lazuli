using Lazuli.Data.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using LazuliLibrary.Utils;

namespace LazuliTest;

public class UserContextTest
{
    [Fact]
    public void TestGetUser()
    {
        using var context = new TestContext();

        // TODO somehow mock this (couldn't find anything about how to do that as of yet)
        // creates a database in memory instead of using an actual database
        // might create unexpected behaviour when done in several tests, especially if run asynchronously
        context.Services.AddDbContextFactory<UserContext>(
            opt => opt.UseInMemoryDatabase("TestGetUser")
        );

        // create scope of factory service and use it to create database context
        using var scope = context.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var userDbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<UserContext>>();
        var userContext = userDbFactory.CreateDbContext();

        // check no user in the database
        Assert.Equal(0, userContext.Users?.Count());

        string username = "username";
        string password = "passwd";
        int boundToUserId = 3;

        userContext.Add(new User(username, password, boundToUserId));
        userContext.SaveChanges();

        // check if only 1 user was added to database
        Assert.Equal(1, userContext.Users?.Count());

        byte[] hashed_password = CipherUtility.Encrypt(password, username);

        // check if added user has expected data
        Assert.True(userContext.Users?.Any(user => user.Login == username && user.Password == hashed_password && user.BoundToUserId == boundToUserId));

        // get user that was added
        var added_user = userContext.GetUser(username, password);

        // get user that wasn't added
        var not_added_user = userContext.GetUser($"{username}NOT!", $"{password}NOT!");

        // check if received user that was added has expected data
        Assert.Equal(username, added_user!.Login);
        Assert.Equal(hashed_password, added_user!.Password);
        Assert.Equal(boundToUserId, added_user!.BoundToUserId);


        // check if received user that wasn't added has null value
        Assert.Null( not_added_user );

        userContext.Dispose();
    }
}
