using LazuliLibrary.API.Endpoints;
using LazuliLibrary.Authentication;
using LazuliLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace LazuliLibrary.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MainController : ControllerBase
{
    private readonly IPostEndpoint _postEndpoint;
    private readonly IUserAuthenticationStateProvider _authenticationStateProvider;

    public MainController(
        IPostEndpoint postEndpoint
        , IUserAuthenticationStateProvider authenticationStateProvider
        )
    {
        _postEndpoint = postEndpoint;
        _authenticationStateProvider = authenticationStateProvider;
    }

    [HttpGet("posts")]
    public async Task<List<PostModel>> GetPosts()
    {
        var posts = await _postEndpoint.GetPartially();
        if (posts is null || posts.Count == 0)
        {
            _postEndpoint.StartIndex = 1;
            posts = await _postEndpoint.GetPartially();
        }

        return posts;
    }
}
