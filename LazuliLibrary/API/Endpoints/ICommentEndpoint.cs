using LazuliLibrary.Models;

namespace LazuliLibrary.API.Endpoints
{
    public interface ICommentEndpoint
    {
        Task<List<CommentModel>> GetAll();
        Task<CommentModel?> GetByCommentId(int commentId);
        Task<List<CommentModel>> GetByUserId(int userId);
    }
}