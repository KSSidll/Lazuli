﻿using LazuliLibrary.Models;

namespace LazuliLibrary.Authentication;

public interface IUserAuthenticationStateProvider
{
	/// <summary>
	/// Returns whether a user is currently logged in or not
	/// </summary>
	public Task<bool> IsAuthenticated();

	public Task Logout();
	public Task Login(AuthenticatedUserModel user);

	/// <summary>
	/// Returns boundToUserId of currently logged in user
	/// </summary>
	public Task<int> GetBoundToUserId();

	/// <summary>
	/// Returns email of currently logged in user
	/// </summary>
	public Task<string> GetUserEmail();

	/// <summary>
	///     Update the state of the authentication to passed AuthenticatedUserModel, pass null to logout
	/// </summary>
	/// <param name="userSession">AuthenticatedUserModel with data to use as user, null to logout</param>
	public Task UpdateAuthenticationState(AuthenticatedUserModel? userSession);
}