using LazuliLibrary.Models;
using LazuliLibrary.API;
using LazuliLibrary.API.Endpoints;

namespace Lazuli.Service;

// not sure how to do it differently with tools i was given, so here we go
public class UserService
{
    UserEndpoint? userEndpoint;

    public void setApihelper(ApiHelper api)
    {
        userEndpoint = new UserEndpoint(api);
    }

    public async Task<List<UserModel>> GetAll()
    {
        return await userEndpoint!.GetAll();
    }
}
