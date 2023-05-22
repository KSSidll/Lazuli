using LazuliLibrary.API;
using LazuliLibrary.API.Endpoints;
using LazuliLibrary.Authentication;
using LazuliLibrary.Data.Database;
using LazuliLibrary.Data.Utils;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<IUserAuthenticationStateProvider, UserAuthenticationStateProvider>();

// Controllers
builder.Services.AddControllers();

WebApplication app = builder.Build();

await using AsyncServiceScope scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
var options = scope.ServiceProvider.GetRequiredService<DbContextOptions<UserContext>>();
await DatabaseUtility.EnsureUserDbCreatedAsync(options);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Special/Error");

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/Special/_Host");
app.MapControllers();

app.Run();