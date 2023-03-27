using Microsoft.EntityFrameworkCore;
using LazuliLibrary.API;
using Lazuli.Data.Database;
using Lazuli.Utils;
using Lazuli.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContextFactory<UserContext>(
    opt => opt.UseSqlite($"Data Source={nameof(UserContext.UserDb)}.db")
);
builder.Services.AddSingleton<UserService>();

var app = builder.Build();

await using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
var options = scope.ServiceProvider.GetRequiredService<DbContextOptions<UserContext>>();
await DatabaseUtility.EnsureUserDbCreatedAsync(options);

var api = new ApiHelper();
var userService = app.Services.GetRequiredService<UserService>();
userService.setApihelper(api);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Special/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/Special/_Host");

app.Run();
