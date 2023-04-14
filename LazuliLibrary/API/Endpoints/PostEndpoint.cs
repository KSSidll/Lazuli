using LazuliLibrary.Models;

namespace LazuliLibrary.API.Endpoints;

public class PostEndpoint : IPostEndpoint
{
	private const string Page = "posts";
	private readonly IApiHelper _apiHelper;

	public PostEndpoint(IApiHelper apiHelper)
	{
		_apiHelper = apiHelper;
	}

	public int RecordLimit { get; set; } = 10;

	public int StartIndex { get; set; } = 1;

	/// <summary>
	/// If an empty list is returned you can change the value
	/// of StartIndex or RecordLimit (e.g. StartIndex = 1)
	/// </summary>
	/// <returns></returns>
	/// <exception cref="HttpRequestException"></exception>
	public async Task<List<PostModel>> GetPartially()
	{
		// checks if there are null values
		ApiHelper.ApiHelperValidator(_apiHelper);

		var url = $"/{Page}?_start={StartIndex}&_limit={RecordLimit}";

		using HttpResponseMessage response = await _apiHelper.ApiClient!.GetAsync(url);

		if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.ReasonPhrase);

		var result = await response.Content.ReadAsAsync<List<PostModel>>();

		StartIndex += RecordLimit;

		return result;
	}

	public async Task<List<PostModel>> GetByBodyFuzzy(string body)
	{
		// checks if there are null values
		ApiHelper.ApiHelperValidator(_apiHelper);

		using HttpResponseMessage response = await _apiHelper.ApiClient!.GetAsync($"/{Page}?body_like={body}");

		if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.ReasonPhrase);

		var result = await response.Content.ReadAsAsync<List<PostModel>>();

		return result;
	}

	public async Task<List<PostModel>> GetAll()
	{
		// checks if there are null values
		ApiHelper.ApiHelperValidator(_apiHelper);

		using HttpResponseMessage response = await _apiHelper.ApiClient!.GetAsync($"/{Page}");

		if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.ReasonPhrase);

		var result = await response.Content.ReadAsAsync<List<PostModel>>();

		return result;
	}


	public async Task<PostModel?> GetByPostId(int postId)
	{
		// checks if there are null values
		ApiHelper.ApiHelperValidator(_apiHelper);

		using HttpResponseMessage response = await _apiHelper.ApiClient!.GetAsync($"/{Page}?id={postId}");

		if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.ReasonPhrase);

		var result = await response.Content.ReadAsAsync<List<PostModel>>();

		if (result.Count > 1) throw new Exception("More than one matching object found.");

		return result.FirstOrDefault(defaultValue: null);
	}

	public async Task<List<PostModel>> GetByUserId(int userId)
	{
		// checks if there are null values
		ApiHelper.ApiHelperValidator(_apiHelper);

		using HttpResponseMessage response = await _apiHelper.ApiClient!.GetAsync($"/{Page}?userId={userId}");

		if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.ReasonPhrase);

		var result = await response.Content.ReadAsAsync<List<PostModel>>();

		return result;
	}
}