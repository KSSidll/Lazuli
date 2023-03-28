using LazuliLibrary.Models;

namespace LazuliLibrary.API
{
    public interface IUserEndpoint
    {
        Task<List<UserModel>> GetAll();
        Task<UserModel> GetByUserId(int userId);
    }
}