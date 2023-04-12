using LazuliLibrary.API.Endpoints;
using LazuliLibrary.Models;
using LazuliTest.Helpers;

namespace LazuliTest.Fakes;

public class FakePostEndpoint : IPostEndpoint
{
	public int RecordLimit { get; set; } = 5;
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
		return (List<PostModel>) await Task.Run(() =>
		{
			return TestDataHelper.GetFakePostModelList()
								 .Where(x => x.UserId == userId);
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
}