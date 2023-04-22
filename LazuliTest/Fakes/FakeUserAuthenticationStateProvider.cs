using LazuliLibrary.Authentication;
using LazuliLibrary.Models;

namespace LazuliTest.Fakes;

// unfortunately it looks like making a fake state provider might be the only option for test use case
public class FakeUserAuthenticationStateProvider : IUserAuthenticationStateProvider
{
	private int _boundToUserId;
	private string _userEmail = string.Empty;

	public Task<bool> IsAuthenticated()
	{
		return Task.Run(() => _boundToUserId != 0);
	}

	public Task Logout()
	{
		return Task.Run(() =>
		{
			_boundToUserId = 0;
			_userEmail = string.Empty;
		});
	}

	public Task Login(AuthenticatedUserModel user)
	{
		return Task.Run(() =>
		{
			_boundToUserId = int.Parse(user.BoundToUserId!);
			_userEmail = user.Email!;
		});
	}

	public Task<int> GetBoundToUserId()
	{
		return Task.Run(() => _boundToUserId);
	}

	public Task<string> GetUserEmail()
	{
		return Task.Run(() => _userEmail);
	}

	public Task UpdateAuthenticationState(AuthenticatedUserModel? userSession)
	{
		return Task.Run(() =>
		{
			if (userSession?.BoundToUserId == null)
			{
				_boundToUserId = 0;
				_userEmail = string.Empty;
			}
			else
			{
				_boundToUserId = int.Parse(userSession.BoundToUserId);
				_userEmail = userSession.Email!;
			}
		});
	}
}