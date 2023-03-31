using LazuliLibrary.Models;

namespace LazuliLibrary.API.Endpoints
{
    public interface ICommentEndpoint
    {
        Task<List<CommentModel>> GetAll();
        Task<List<CommentModel>> GetByCommentId(int commentId);
        Task<List<CommentModel>> GetByUserId(int userId);
    }
}