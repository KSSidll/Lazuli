using Lazuli.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace Lazuli.Utils;

public static class DatabaseUtility
{
	public static async Task EnsureUserDbCreatedAsync(DbContextOptions<UserContext> options)
	{
		var builder = new DbContextOptionsBuilder<UserContext>(options);

		await using var context = new UserContext(builder.Options);
		await context.Database.EnsureCreatedAsync();
	}
}