using AngleSharp.Io;
using LazuliLibrary.Models;
using LazuliTest.TestDataHelper;

namespace LazuliLibrary.API.Endpoints;

public class FakeUserEndpoint : IUserEndpoint
{
    public async Task<List<UserModel>> GetAll()
    {
        return TestDataHelper.GetFakeUserModelList();
    }


    public async Task<UserModel> GetByUserId(int userId)
    {
        var data = TestDataHelper.GetFakeUserModelList().Find(user => user.Id == userId);

        if (data is null) throw new HttpRequestException();

        return data;
    }
}

