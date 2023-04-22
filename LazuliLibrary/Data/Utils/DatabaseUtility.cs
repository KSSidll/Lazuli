using LazuliLibrary.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace LazuliLibrary.Data.Utils;

public static class DatabaseUtility
{
	public static async Task EnsureUserDbCreatedAsync(DbContextOptions<UserContext> options)
	{
		var builder = new DbContextOptionsBuilder<UserContext>(options);

		await using var context = new UserContext(builder.Options);
		await context.Database.EnsureCreatedAsync();
	}
}