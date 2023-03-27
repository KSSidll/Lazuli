using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
        base.Dispose();
    }

    /// <summary>
    /// Dispose pattern.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/></returns>
    public override ValueTask DisposeAsync()
    {
        Debug.WriteLine($"{ContextId} context disposed async.");
        return base.DisposeAsync();
    }
}