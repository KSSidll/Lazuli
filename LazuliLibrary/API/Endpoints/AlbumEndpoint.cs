using LazuliLibrary.Models;

namespace LazuliLibrary.API.Endpoints;

public class AlbumEndpoint : IAlbumEndpoint
{
	private const string Page = "albums";
	private readonly IApiHelper _apiHelper;

	public AlbumEndpoint(IApiHelper apiHelper)
	{
		_apiHelper = apiHelper;
	}

	public async Task<List<AlbumModel>> GetAll()
	{
		// checks if there are null values
		ApiHelper.ApiHelperValidator(_apiHelper);

		using HttpResponseMessage response = await _apiHelper.ApiClient!.GetAsync($"/{Page}");

		if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.ReasonPhrase);

		var result = await response.Content.ReadAsAsync<List<AlbumModel>>();

		return result;
	}

	public async Task<AlbumModel?> GetByAlbumId(int albumId)
	{
		// checks if there are null values
		ApiHelper.ApiHelperValidator(_apiHelper);

		using HttpResponseMessage response = await _apiHelper.ApiClient!.GetAsync($"/{Page}?id={albumId}");

		if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.ReasonPhrase);

		var result = await response.Content.ReadAsAsync<List<AlbumModel>>();

		if (result.Count > 1) throw new Exception("More than one matching object found.");

		return result.FirstOrDefault(defaultValue: null);
	}

	public async Task<List<AlbumModel>> GetByUserId(int userId)
	{
		// checks if there are null values
		ApiHelper.ApiHelperValidator(_apiHelper);

		using HttpResponseMessage response = await _apiHelper.ApiClient!.GetAsync($"/{Page}?userId={userId}");

		if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.ReasonPhrase);

		var result = await response.Content.ReadAsAsync<List<AlbumModel>>();

		return result;
	}
}