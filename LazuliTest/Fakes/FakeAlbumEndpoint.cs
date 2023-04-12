using LazuliLibrary.API.Endpoints;
using LazuliLibrary.Models;
using LazuliTest.Helpers;

namespace LazuliTest.Fakes;

public class FakeAlbumEndpoint : IAlbumEndpoint
{
	public async Task<List<AlbumModel>> GetAll()
	{
		return await Task.Run(TestDataHelper.GetFakeAlbumModelList);
	}

	public async Task<AlbumModel?> GetByAlbumId(int albumId)
	{
		return await Task.Run(() => { return TestDataHelper.GetFakeAlbumModelList().First(x => x.Id == albumId); });
	}

	public async Task<List<AlbumModel>> GetByUserId(int userId)
	{
		return await Task.Run(() =>
		{
			return TestDataHelper.GetFakeAlbumModelList().Where(x => x.UserId == userId).ToList();
		});
	}
}