using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Models.DTO;
using EntertaimentService.Models.Photos.Requests;

namespace EntertaimentService.Services.PhotoService
{
    public interface IPhotoService
    {
        Task<long> AddPhoto(AddPhotoRequest addPhotoRequest);
        Task<PhotoDto> GetPhoto(GetPhotoRequest getPhotoRequest);
        IQueryable<PhotoDto> GetPhotos(GetPhotosRequest getPhotosRequest);
        bool RemovePhoto(RemovePhotoRequest removePhotoRequest);
        bool UpdatePhoto(UpdatePhotoRequest updatePhotoRequest);
    }
}