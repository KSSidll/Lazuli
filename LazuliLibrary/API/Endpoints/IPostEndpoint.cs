using LazuliLibrary.Models;

namespace LazuliLibrary.API.Endpoints
{
    public interface IPostEndpoint
    {
        Task<List<PostModel>> GetAll();
        Task<PostModel?> GetByPostId(int postId);
        Task<List<PostModel>> GetByUserId(int userId);
    }
}