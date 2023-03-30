using LazuliLibrary.Models;

namespace Lazuli.Authentication;

public interface IUserAuthenticationStateProvider
{
    // TODO add method that returns BoundToUserId of currently logged in user as int 

    /// <summary>
    /// Returns whether a user is currently logged in or not
    /// </summary>
    public Task<bool> IsAuthenticated();
    public Task Logout();
    public Task Login(int boundToUserId);

    /// <summary>
    ///     Update the state of the authentication to passed AuthenticatedUserModel, pass null to logout
    /// </summary>
    /// <param name="userSession">AuthenticatedUserModel with data to use as user, null to logout</param>
    public Task UpdateAuthenticationState(AuthenticatedUserModel? userSession);
}
