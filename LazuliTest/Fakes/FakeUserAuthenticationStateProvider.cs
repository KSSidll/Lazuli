using System.Security.Claims;
using Lazuli.Authentication;
using LazuliLibrary.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.DataProtection;

namespace LazuliTest.Fakes;

// unfortunately it looks like making a fake state provider might be the only option for test use case
public class FakeUserAuthenticationStateProvider : UserAuthenticationStateProvider, IUserAuthenticationStateProvider
{
    public FakeUserAuthenticationStateProvider() : base(new ProtectedSessionStorage(new BunitJSInterop().JSRuntime, new EphemeralDataProtectionProvider())) { }

    private int _boundToUserId = 0;
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public new Task<bool> IsAuthenticated()
    {
        return Task.Run(() => _boundToUserId != 0);
    }

    public new Task Logout()
    {
        return Task.Run(() => _boundToUserId = 0);
    }
    public new Task Login(int boundToUserId)
    {
        return Task.Run(() => _boundToUserId = boundToUserId);
    }

    public new Task<int> GetBoundToUserId()
    {
        return Task.Run(() => _boundToUserId);
    }
    
    public new Task UpdateAuthenticationState(AuthenticatedUserModel? userSession)
    {
        return Task.Run(() =>
        {
            if (userSession == null || userSession.BoundToUserId == null)
            {
                _boundToUserId = 0;
            }
            else
            {
                _boundToUserId = int.Parse(userSession.BoundToUserId);
            }
        });
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_boundToUserId == 0)
            return await Task.FromResult(new AuthenticationState(_anonymous));

        AuthenticatedUserModel userSession = new()
        {
            BoundToUserId = _boundToUserId.ToString()
        };

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
}