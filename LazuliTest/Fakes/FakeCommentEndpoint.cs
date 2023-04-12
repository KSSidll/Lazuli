using LazuliLibrary.API.Endpoints;
using LazuliLibrary.Models;
using LazuliTest.Helpers;

namespace LazuliTest.Fakes;

public class FakeCommentEndpoint : ICommentEndpoint
{
	public async Task<List<CommentModel>> GetAll()
	{
		return await Task.Run(TestDataHelper.GetFakeCommentModelList);
	}

	public async Task<CommentModel?> GetByCommentId(int commentId)
	{
		return await Task.Run(() => { return TestDataHelper.GetFakeCommentModelList().First(x => x.Id == commentId); });
	}

	public async Task<List<CommentModel>> GetByPostId(int postId)
	{
		return await Task.Run(() =>
		{
			return TestDataHelper.GetFakeCommentModelList().Where(x => x.PostId == postId)
								 .ToList();
		});
	}
}