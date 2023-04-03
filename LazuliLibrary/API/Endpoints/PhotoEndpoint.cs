using LazuliLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazuliLibrary.API.Endpoints
{
    public class PhotoEndpoint : IPhotoEndpoint
    {
        private readonly IApiHelper _apiHelper;
        private const string _page = "photos";

        public PhotoEndpoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<PhotoModel>> GetAll()
        {
            // checks if there are null values
            ApiHelper.ApiHelperValidator(_apiHelper);

            using (HttpResponseMessage response = await _apiHelper!.ApiClient!.GetAsync($"/{_page}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<PhotoModel>>();

                    return result;
                }
                else
                {
                    throw new HttpRequestException(response.ReasonPhrase);
                }
            }
        }

        public async Task<PhotoModel?> GetByPhotoId(int photoId)
        {
            // checks if there are null values
            ApiHelper.ApiHelperValidator(_apiHelper);

            using (HttpResponseMessage response = await _apiHelper!.ApiClient!.GetAsync($"/{_page}?id={photoId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<PhotoModel>>();

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

        public async Task<List<PhotoModel>> GetByAlbumId(int albumId)
        {
            // checks if there are null values
            ApiHelper.ApiHelperValidator(_apiHelper);

            using (HttpResponseMessage response = await _apiHelper!.ApiClient!.GetAsync($"/{_page}?albumId={albumId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<PhotoModel>>();

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
