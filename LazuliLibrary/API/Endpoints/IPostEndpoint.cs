using LazuliLibrary.Models;

namespace LazuliLibrary.API.Endpoints;

public interface IPostEndpoint
{
	int RecordLimit { get; set; }
	int StartIndex { get; set; }
	Task<List<PostModel>> GetAll();
	Task<PostModel?> GetByPostId(int postId);
	Task<List<PostModel>> GetByUserId(int userId);
	Task<List<PostModel>> GetPartially();
}