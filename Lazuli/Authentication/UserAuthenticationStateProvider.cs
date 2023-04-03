using System.Security.Claims;
using LazuliLibrary.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Lazuli.Authentication;

public class UserAuthenticationStateProvider : AuthenticationStateProvider, IUserAuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private readonly ClaimsPrincipal _anonymous = new (new ClaimsIdentity());

    public UserAuthenticationStateProvider(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    
    public async Task<bool> IsAuthenticated()
    {
        var state = await GetAuthenticationStateAsync();
        return state.User.Identity!.IsAuthenticated;

    }

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

    public async Task<int> GetBoundToUserId()
    {
        if (!await IsAuthenticated()) return 0;

        var boundToUserId = (await GetAuthenticationStateAsync()).User.FindFirst(x => x.Type == ClaimTypes.Actor)!.Value;

        return int.Parse(boundToUserId!);
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