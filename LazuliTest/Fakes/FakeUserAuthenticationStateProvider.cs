using LazuliLibrary.Authentication;
using LazuliLibrary.Models;

namespace LazuliTest.Fakes;

// unfortunately it looks like making a fake state provider might be the only option for test use case
public class FakeUserAuthenticationStateProvider : IUserAuthenticationStateProvider
{
	private int _boundToUserId;

	public Task<bool> IsAuthenticated()
	{
		return Task.Run(() => _boundToUserId != 0);
	}

	public Task Logout()
	{
		return Task.Run(() => _boundToUserId = 0);
	}

	public Task Login(int boundToUserId)
	{
		return Task.Run(() => _boundToUserId = boundToUserId);
	}

	public Task<int> GetBoundToUserId()
	{
		return Task.Run(() => _boundToUserId);
	}

	public Task UpdateAuthenticationState(AuthenticatedUserModel? userSession)
	{
		return Task.Run(() =>
		{
			if (userSession?.BoundToUserId == null)
				_boundToUserId = 0;
			else
				_boundToUserId = int.Parse(userSession.BoundToUserId);
		});
	}
}