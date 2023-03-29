using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using LazuliLibrary.Utils;

namespace Lazuli.Data.Database;

/// <summary>
/// Context for the user database.
/// </summary>
public class UserContext : DbContext
{
    /// <summary>
    /// Magic string.
    /// </summary>
    public static readonly string RowVersion = nameof(RowVersion);

    /// <summary>
    /// Magic strings.
    /// </summary>
    public static readonly string UserDb = nameof(UserDb).ToLower();

    public UserContext(DbContextOptions<UserContext> options)
        : base(options) { }

    /// <summary>
    /// List of <see cref="User"/>.
    /// </summary>
    public DbSet<User>? Users { get; set; }

    // TODO abstract this as function that gets login and password, checks if they exist in database and if yes, return auth token
    public User? GetUser(string username, string password)
    {
        byte[] hashed_password = CipherUtility.Encrypt(password, username);

        var user = Users?.Where(user => user.Login == username && user.Password == hashed_password).FirstOrDefault();

        return user;
    }

    /// <summary>
    /// Define the model.
    /// </summary>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/>.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // this property isn't on the C# class
        // so we set it up as a "shadow" property and use it for concurrency
        modelBuilder.Entity<User>().Property<byte[]>(RowVersion).IsRowVersion();

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Dispose pattern.
    /// </summary>
    public override void Dispose()
    {
        Debug.WriteLine($"{ContextId} context disposed.");
        GC.SuppressFinalize(this);
        base.Dispose();
    }

    /// <summary>
    /// Dispose pattern.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/></returns>
    public override ValueTask DisposeAsync()
    {
        Debug.WriteLine($"{ContextId} context disposed async.");
        GC.SuppressFinalize(this);
        return base.DisposeAsync();
    }
}
