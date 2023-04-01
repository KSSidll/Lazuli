﻿using LazuliLibrary.API.Endpoints;
using LazuliLibrary.Models;
using LazuliTest.Helpers;

namespace LazuliTest.Fakes;

public class FakeUserEndpoint : IUserEndpoint
{
    public async Task<List<UserModel>> GetAll()
    {
        return await Task.Run(() =>
        {
            return TestDataHelper.GetFakeUserModelList();
        });
    }


    public async Task<UserModel> GetByUserId(int userId)
    {
        return await Task.Run(() =>
        {
            var data = TestDataHelper.GetFakeUserModelList().Find(user => user.Id == userId);

            if (data is null) throw new HttpRequestException();

            return data;
        });
    }
}

