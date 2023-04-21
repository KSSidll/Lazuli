﻿using LazuliLibrary.Models;

namespace LazuliLibrary.API.Endpoints;

public class CommentEndpoint : ICommentEndpoint
{
	private const string Page = "comments";
	private readonly IApiHelper _apiHelper;

	public CommentEndpoint(IApiHelper apiHelper)
	{
		_apiHelper = apiHelper;
	}

	public async Task<List<CommentModel>> GetAll()
	{
		// checks if there are null values
		ApiHelper.ApiHelperValidator(_apiHelper);

		using HttpResponseMessage response = await _apiHelper.ApiClient!.GetAsync($"/{Page}");

		if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.ReasonPhrase);

		var result = await response.Content.ReadAsAsync<List<CommentModel>>();

		return result;
	}

	public async Task<CommentModel?> GetByCommentId(int commentId)
	{
		// checks if there are null values
		ApiHelper.ApiHelperValidator(_apiHelper);

		using HttpResponseMessage response = await _apiHelper.ApiClient!.GetAsync($"/{Page}?id={commentId}");

		if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.ReasonPhrase);

		var result = await response.Content.ReadAsAsync<List<CommentModel>>();

		if (result.Count > 1) throw new Exception("More than one matching object found.");

		return result.FirstOrDefault(defaultValue: null);
	}

	public async Task<List<CommentModel>> GetByPostId(int postId)
	{
		// checks if there are null values
		ApiHelper.ApiHelperValidator(_apiHelper);

		using HttpResponseMessage response = await _apiHelper.ApiClient!.GetAsync($"/{Page}?postId={postId}");

		if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.ReasonPhrase);

		var result = await response.Content.ReadAsAsync<List<CommentModel>>();

		return result;
	}

	public async Task<List<CommentModel>> GetByBodyFuzzy(string body)
	{
		// checks if there are null values
		ApiHelper.ApiHelperValidator(_apiHelper);

		using HttpResponseMessage response = await _apiHelper.ApiClient!.GetAsync($"/{Page}?body_like={body}");

		if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.ReasonPhrase);

		var result = await response.Content.ReadAsAsync<List<CommentModel>>();

		return result;
	}

    public async Task DeleteByCommentId(int commentId)
    {
        // todo check if user owns this comment

        // checks if there are null values
        ApiHelper.ApiHelperValidator(_apiHelper);

        using HttpResponseMessage response = await _apiHelper.ApiClient!.DeleteAsync($"/{Page}/{commentId}");

        if (!response.IsSuccessStatusCode) throw new HttpRequestException(response.ReasonPhrase);
    }
}