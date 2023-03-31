using LazuliLibrary.Models;

namespace LazuliLibrary.API.Endpoints
{
    public interface IUserEndpoint
    {
        Task<List<UserModel>> GetAll();
        Task<List<UserModel>> GetByUserId(int userId);
    }
}