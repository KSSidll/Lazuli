using LazuliLibrary.Models;

namespace LazuliLibrary.API.Endpoints
{
    public interface IPhotoEndpoint
    {
        Task<List<PhotoModel>> GetAll();
        Task<List<PhotoModel>> GetByPhotoId(int photoId);
        Task<List<PhotoModel>> GetByPostId(int postId);
    }
}