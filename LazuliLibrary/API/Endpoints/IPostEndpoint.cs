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
	Task<List<PostModel>> GetByBodyFuzzy(string body);
	Task DeleteByPostId(int postId);
	public Task<PostModel> CreatePost(string body, string title);
	Task<bool> PatchPostBodyByPostId(int postId, string body);
    Task<List<PostModel>> GetByCharacterCountInBodyAndBodyFuzzy(int? lower = 0, int? upper = null, string? body = "");
}