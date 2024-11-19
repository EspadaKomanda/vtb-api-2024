using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Models.Photos.Requests;
using EntertaimentService.Models.Photos.Responses;

namespace ApiGatewayService.Services.EntertaimentService.Photos
{
    public interface IEntertaimentPhotoService
    {
        Task<AddPhotoResponse> AddPhoto(AddPhotoEntertaimentRequest request);
        Task<GetPhotoResponse> GetPhoto(GetPhotoEntertainmentRequest request);
        Task<RemovePhotoResponse> RemovePhoto(RemovePhotoEntertainmentRequest request);
        Task<UpdatePhotoResponse> UpdatePhoto(UpdatePhotoEntertainmentRequest request);
        Task<GetPhotosResponse> GetPhotos(GetPhotosEntertaimentRequest request);
    }
}