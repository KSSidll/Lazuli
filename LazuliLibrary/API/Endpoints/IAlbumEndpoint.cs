﻿using LazuliLibrary.Models;

namespace LazuliLibrary.API.Endpoints
{
    public interface IAlbumEndpoint
    {
        Task<List<AlbumModel>> GetAll();
        Task<List<AlbumModel>> GetByAlbumId(int albumId);
        Task<List<AlbumModel>> GetByUserId(int userId);
    }
}