using LazuliLibrary.Models;

namespace LazuliLibrary.API;

public class UserEndpoint
{
    private readonly ApiHelper _apiHelper;

    public UserEndpoint(ApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    public async Task<List<UserModel>> GetAll()
    {
        using (HttpResponseMessage response = await _apiHelper!.ApiClient!.GetAsync("/users"))
        {
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<List<UserModel>>();

                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
