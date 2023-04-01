using Microsoft.EntityFrameworkCore;
using LazuliLibrary.API;
using Lazuli.Data.Database;
using Lazuli.Utils;
using LazuliLibrary.API.Endpoints;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Lazuli.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Database Service
builder.Services.AddDbContextFactory<UserContext>(
    opt => opt.UseSqlite($"Data Source={nameof(UserContext.UserDb)}.db")
);

// API Endpoints
builder.Services.AddTransient<IApiHelper, ApiHelper>();
builder.Services.AddTransient<IUserEndpoint, UserEndpoint>();
builder.Services.AddTransient<IPostEndpoint, PostEndpoint>();
builder.Services.AddTransient<IAlbumEndpoint, AlbumEndpoint>();
builder.Services.AddTransient<ICommentEndpoint, CommentEndpoint>();
builder.Services.AddTransient<IPhotoEndpoint, PhotoEndpoint>();

// Authentication
builder.Services.AddAuthenticationCore();
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, UserAuthenticationStateProvider>();

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
