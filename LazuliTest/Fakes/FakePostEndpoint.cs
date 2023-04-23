using LazuliLibrary.API.Endpoints;
using LazuliLibrary.Models;
using LazuliTest.Helpers;

namespace LazuliTest.Fakes;

public class FakePostEndpoint : IPostEndpoint
{
	public int RecordLimit { get; set; } = 2;
	public int StartIndex { get; set; } = 1;

	public async Task<List<PostModel>> GetAll()
	{
		return await Task.Run(TestDataHelper.GetFakePostModelList);
	}

	public async Task<PostModel?> GetByPostId(int postId)
	{
		return await Task.Run(() => { return TestDataHelper.GetFakePostModelList().First(x => x.Id == postId); });
	}

	public async Task<List<PostModel>> GetByUserId(int userId)
	{
		return await Task.Run(() =>
		{
			return TestDataHelper.GetFakePostModelList().Where(x => x.UserId == userId).ToList();
		});
	}

	public async Task<List<PostModel>> GetPartially()
	{
		return await Task.Run(() =>
		{
			List<PostModel> result = TestDataHelper.GetFakePostModelList().GetRange(StartIndex - 1, RecordLimit);

			StartIndex += RecordLimit;

			return result;
		});
	}

	public async Task<List<PostModel>> GetByBodyFuzzy(string body)
	{
		return await Task.Run(() =>
		{
			return TestDataHelper.GetFakePostModelList().Where(x => x.Body!.Contains(body))
								 .ToList();
		});
	}

	// shouldn't ever be called in tests
	public Task DeleteByPostId(int postId)
	{
		throw new NotImplementedException();
	}

	// shouldn't ever be called in tests
	public Task<bool> CreatePost(string body, string title)
	{
		throw new NotImplementedException();
	}

	// shouldn't ever be called in tests
	public Task<bool> PatchPostBodyByPostId(int postId, string body)
	{
		throw new NotImplementedException();
	}
}