using Microsoft.EntityFrameworkCore;
using LazuliLibrary.API;
using Lazuli.Data.Database;
using Lazuli.Utils;
using LazuliLibrary.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Personal Services
builder.Services.AddDbContextFactory<UserContext>(
    opt => opt.UseSqlite($"Data Source={nameof(UserContext.UserDb)}.db")
);
builder.Services.AddTransient<IApiHelper, ApiHelper>();
builder.Services.AddTransient<IUserEndpoint, UserEndpoint>();
builder.Services.AddTransient<IPostEndpoint, PostEndpoint>();
builder.Services.AddTransient<IAlbumEndpoint, AlbumEndpoint>();
builder.Services.AddTransient<ICommentEndpoint, CommentEndpoint>();
builder.Services.AddTransient<IPhotoEndpoint, PhotoEndpoint>();

var app = builder.Build();

await using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
var options = scope.ServiceProvider.GetRequiredService<DbContextOptions<UserContext>>();
await DatabaseUtility.EnsureUserDbCreatedAsync(options);

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
