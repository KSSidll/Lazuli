using LazuliLibrary.Models;

namespace LazuliLibrary.API.Endpoints;

public interface ICommentEndpoint
{
	Task<List<CommentModel>> GetAll();
	Task<CommentModel?> GetByCommentId(int commentId);
	Task<List<CommentModel>> GetByPostId(int postId);
	Task<List<CommentModel>> GetByBodyFuzzy(string body);
	Task DeleteByCommentId(int commentId);
}