using LazuliLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazuliLibrary.API.Endpoints
{
    public class AlbumEndpoint : IAlbumEndpoint
    {
        private readonly IApiHelper _apiHelper;
        private const string _page = "albums";

        public AlbumEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<AlbumModel>> GetAll()
        {
            // checks if there are null values
            ApiHelper.ApiHelperValidator(_apiHelper);

            using (HttpResponseMessage response = await _apiHelper!.ApiClient!.GetAsync($"/{_page}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<AlbumModel>>();

                    return result;
                }
                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }
            }
        }

        public async Task<AlbumModel?> GetByAlbumId(int albumId)
        {
            // checks if there are null values
            ApiHelper.ApiHelperValidator(_apiHelper);

            using (HttpResponseMessage response = await _apiHelper!.ApiClient!.GetAsync($"/{_page}?id={albumId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<AlbumModel>>();

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

        public async Task<List<AlbumModel>> GetByUserId(int userId)
        {
            // checks if there are null values
            ApiHelper.ApiHelperValidator(_apiHelper);

            using (HttpResponseMessage response = await _apiHelper!.ApiClient!.GetAsync($"/{_page}?userId={userId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<AlbumModel>>();

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
