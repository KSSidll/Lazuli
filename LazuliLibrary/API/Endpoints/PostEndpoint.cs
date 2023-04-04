using LazuliLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazuliLibrary.API.Endpoints
{
    public class PostEndpoint : IPostEndpoint
    {
        private readonly IApiHelper _apiHelper;
        private const string _page = "posts";

        public int RecordLimit { get; set; } = 10;

        public int StartIndex { get; set; } = 1;

		public PostEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

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

            string url = $"/{_page}?_start={StartIndex}&_limit={RecordLimit}";

            using (HttpResponseMessage response = await _apiHelper!.ApiClient!.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<PostModel>>();

                    StartIndex += RecordLimit;

                    return result;
                }
                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }
            }
        }
        public async Task<List<PostModel>> GetAll()
        {
            // checks if there are null values
            ApiHelper.ApiHelperValidator(_apiHelper);

            using (HttpResponseMessage response = await _apiHelper!.ApiClient!.GetAsync($"/{_page}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<PostModel>>();

                    return result;
                }
                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }
            }
        }


        public async Task<PostModel?> GetByPostId(int postId)
        {
            // checks if there are null values
            ApiHelper.ApiHelperValidator(_apiHelper);

            using (HttpResponseMessage response = await _apiHelper!.ApiClient!.GetAsync($"/{_page}?id={postId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<PostModel>>();

                    if (result.Count > 1)
                    {
                        throw new Exception("More than one matching object found.");
                    }

                    return result.FirstOrDefault(defaultValue: null);
                }
                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }
            }
        }
        public async Task<List<PostModel>> GetByUserId(int userId)
        {
            // checks if there are null values
            ApiHelper.ApiHelperValidator(_apiHelper);

            using (HttpResponseMessage response = await _apiHelper!.ApiClient!.GetAsync($"/{_page}?userId={userId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<PostModel>>();

                    return result;
                }
                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }
            }
        }

    }
}
