using LazuliLibrary.API.Endpoints;
using LazuliLibrary.Authentication;
using LazuliLibrary.Data.Database;
using LazuliLibrary.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LazuliLibrary.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
	private readonly IUserAuthenticationStateProvider _authenticationStateProvider;
	private readonly UserContext _userContext;
	private readonly IUserEndpoint _userEndpoint;

	public AuthenticationController(IUserAuthenticationStateProvider authenticationStateProvider,
									IDbContextFactory<UserContext> userContextFactory, IUserEndpoint userEndpoint)
	{
		_authenticationStateProvider = authenticationStateProvider;
		_userContext = userContextFactory.CreateDbContext();
		_userEndpoint = userEndpoint;
	}

	[HttpGet("authenticated")]
	public async Task<IActionResult> IsAuthenticated()
	{
		var authenticated = await _authenticationStateProvider.IsAuthenticated();

		if (authenticated) return Ok("authenticated");

		return Ok("not authenticated");
	}

	[HttpPost("register")]
	public async Task<IActionResult> Register(SignupModel model)
	{
		if (!ModelState.IsValid || model.Login is null || model.Password is null) return BadRequest(ModelState);

		_userContext.Add(new User(model.Login, model.Password, model.BoundToUserId));
		await _userContext.SaveChangesAsync();

		User? user = _userContext.GetUser(model.Login, model.Password);

		if (user is null) return StatusCode(500);

		return Ok();
	}

	// [HttpPost("login")]
	// public async Task<IActionResult> Login([FromBody] LoginModel model)
	// {
	// 	if (!ModelState.IsValid || model.Login is null || model.Password is null) return BadRequest(ModelState);
	//
	// 	User? user = _userContext.GetUser(model.Login, model.Password);
	//
	// 	if (user is null) return StatusCode(500);
	//
	// 	UserModel? authUser = await _userEndpoint.GetByUserId(user.BoundToUserId);
	//
	// 	if (authUser is null) return BadRequest();
	//
	// 	AuthenticatedUserModel authUserModel = new()
	// 	{
	// 		BoundToUserId = authUser.Id.ToString(),
	// 		Email = authUser.Email
	// 	};
	//
	// 	await _authenticationStateProvider.Login(authUserModel);
	//
	// 	return Ok();
	// }

	[HttpGet("logout")]
	public async Task<IActionResult> Logout()
	{
		if (await _authenticationStateProvider.IsAuthenticated())
			await _authenticationStateProvider.Logout();

		return Ok();
	}
}