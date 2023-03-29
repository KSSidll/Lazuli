using LazuliLibrary.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace Lazuli.Authentication;

public class UserAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly ClaimsPrincipal _anonymous = new (new ClaimsIdentity());

    public UserAuthenticationStateProvider(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    /// <summary>
    /// Returns whether a user is currently logged in or not
    /// </summary>
    public async Task<bool> IsAuthenticated()
    {
        var state = await GetAuthenticationStateAsync();
        return state.User.Identity!.IsAuthenticated;

    }

    // TODO add method that returns BoundToUserId of currently logged in user as int 

    public async Task Logout()
    {
        await UpdateAuthenticationState(null);
    }

    public async Task Login(int boundToUserId)
    {
        await UpdateAuthenticationState(new AuthenticatedUserModel
        {
            BoundToUserId = boundToUserId.ToString()
        });
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSessionStorageResult = await _sessionStorage.GetAsync<AuthenticatedUserModel>(
                "UserSession"
            );
            var userSession = userSessionStorageResult.Success
                ? userSessionStorageResult.Value
                : null;

            if (userSession is null)
                return await Task.FromResult(new AuthenticationState(_anonymous));

            var claimsPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new List<Claim>
                    {
                        new Claim(ClaimTypes.Actor, userSession.BoundToUserId!),
                    },
                    "UserAuth"
                )
            );
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }
        catch
        {
            return await Task.FromResult(new AuthenticationState(_anonymous));
        }
    }

    /// <summary>
    ///     Update the state of the authentication to passed AuthenticatedUserModel, pass null to logout
    /// </summary>
    /// <param name="userSession">AuthenticatedUserModel with data to use as user, null to logout</param>
    public async Task UpdateAuthenticationState(AuthenticatedUserModel? userSession)
    {
        ClaimsPrincipal claimsPrincipal;

        if (userSession is not null)
        {
            await _sessionStorage.SetAsync("UserSession", userSession);

            claimsPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new List<Claim>
                    {
                        new Claim(ClaimTypes.Actor, userSession.BoundToUserId!),
                    }
                )
            );
        }
        else
        {
            await _sessionStorage.DeleteAsync("UserSession");
            claimsPrincipal = _anonymous;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }
}
