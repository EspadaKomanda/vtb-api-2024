using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGatewayService.Models.TourService.Models.Photos.Requests;
using ApiGatewayService.Models.TourService.Models.Photos.Responses;

namespace ApiGatewayService.Services.TourService.Photos
{
    public interface ITourPhotoService
    {
        Task<AddPhotoResponse> AddPhoto(AddPhotoRequest request);
        Task<GetPhotoResponse> GetPhoto(GetPhotoRequest request);
        Task<RemovePhotoResponse> RemovePhoto(RemovePhotoRequest request);
        Task<UpdatePhotoResponse> UpdatePhoto(UpdatePhotoRequest request);
        Task<GetPhotosResponse> GetPhotos(GetPhotosRequest request);
    }
}