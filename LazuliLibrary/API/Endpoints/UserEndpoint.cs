using LazuliLibrary.Models;

namespace LazuliLibrary.API.Endpoints;

public class UserEndpoint : IUserEndpoint
{
	private readonly IApiHelper _apiHelper;

	public UserEndpoint(IApiHelper apiHelper)
	{
		_apiHelper = apiHelper;
	}

	public async Task<List<UserModel>> GetAll()
	{
		// checks if there are null values
		ApiHelper.ApiHelperValidator(_apiHelper);

		using HttpResponseMessage response = await _apiHelper.ApiClient!.GetAsync("/users");

		if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.ReasonPhrase);

		var result = await response.Content.ReadAsAsync<List<UserModel>>();

		return result;
	}

	public async Task<UserModel?> GetByUserId(int userId)
	{
		// checks if there are null values
		ApiHelper.ApiHelperValidator(_apiHelper);

		using HttpResponseMessage response = await _apiHelper.ApiClient!.GetAsync($"/users?id={userId}");

		if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.ReasonPhrase);

		var result = await response.Content.ReadAsAsync<List<UserModel>>();

		if (result.Count > 1) throw new Exception("More than one matching object found.");

		return result.FirstOrDefault(defaultValue: null);
	}
}