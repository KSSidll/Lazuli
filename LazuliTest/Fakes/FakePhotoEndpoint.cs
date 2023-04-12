using LazuliLibrary.API.Endpoints;
using LazuliLibrary.Models;
using LazuliTest.Helpers;

namespace LazuliTest.Fakes;

public class FakePhotoEndpoint : IPhotoEndpoint
{
	public async Task<List<PhotoModel>> GetAll()
	{
		return await Task.Run(TestDataHelper.GetFakePhotoModelList);
	}

	public async Task<PhotoModel?> GetByPhotoId(int photoId)
	{
		return await Task.Run(() => { return TestDataHelper.GetFakePhotoModelList().First(x => x.Id == photoId); });
	}

	public async Task<List<PhotoModel>> GetByAlbumId(int albumId)
	{
		return (List<PhotoModel>) await Task.Run(() =>
		{
			return TestDataHelper.GetFakePhotoModelList()
								 .Where(x => x.Id == albumId);
		});
	}
}