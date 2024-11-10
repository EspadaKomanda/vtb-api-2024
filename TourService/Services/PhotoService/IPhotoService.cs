using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourService.Models.DTO;
using TourService.Models.Photos.Requests;

namespace TourService.Services.PhotoService
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