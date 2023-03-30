using Lazuli.Authentication;
using LazuliLibrary.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Claims;

namespace LazuliTest.Fakes;

// unfortunately it looks like making a fake state provider might be the only option for test use case
public class FakeUserAuthenticationStateProvider : UserAuthenticationStateProvider
{
    public FakeUserAuthenticationStateProvider() : base(new ProtectedSessionStorage(new BunitJSInterop().JSRuntime, DataProtectionProvider.Create("dummyname"))) { }


    private int boundToUserId = 0;
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public new Task<bool> IsAuthenticated()
    {
        return Task.Run(() => boundToUserId != 0);
    }

    public new Task Logout()
    {
        return Task.Run(() => boundToUserId = 0);
    }
    public new Task Login(int boundToUserId)
    {
        return Task.Run(() => this.boundToUserId = boundToUserId);
    }

    public new Task UpdateAuthenticationState(AuthenticatedUserModel? userSession)
    {
        return Task.Run(() =>
        {
            if (userSession == null || userSession.BoundToUserId == null)
            {
                boundToUserId = 0;
            }
            else
            {
                boundToUserId = int.Parse(userSession.BoundToUserId);
            }
        });
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (boundToUserId == 0)
            return await Task.FromResult(new AuthenticationState(_anonymous));

        AuthenticatedUserModel userSession = new()
        {
            BoundToUserId = boundToUserId.ToString()
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
