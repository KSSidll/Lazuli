using LazuliLibrary.Models;

namespace LazuliLibrary.API.Endpoints;

public interface IPhotoEndpoint
{
	Task<List<PhotoModel>> GetAll();
	Task<PhotoModel?> GetByPhotoId(int photoId);
	Task<List<PhotoModel>> GetByAlbumId(int albumId);
}