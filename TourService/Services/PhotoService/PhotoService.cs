using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TourService.Database.Models;
using TourService.Exceptions.Database;
using TourService.Exceptions.S3ServiceExceptions;
using TourService.Models.DTO;
using TourService.Models.Photos.Requests;
using TourService.Services.S3;
using TourService.Utils.Models;
using UserService.Repositories;

namespace TourService.Services.PhotoService
{
    public class PhotoService(IUnitOfWork unitOfWork, ILogger<PhotoService> logger, IMapper mapper, IS3Service S3Service, IConfiguration configuration) : IPhotoService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<PhotoService> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IS3Service _s3Service = S3Service;
        private readonly IConfiguration _configuration = configuration;
        public async Task<long> AddPhoto(AddPhotoRequest addPhoto)
        {
            try
            {
                List<Bucket> buckets = _configuration.GetSection("Buckets").Get<List<Bucket>>() ?? throw new NullReferenceException();
               
                if(await _s3Service.UploadImageToS3Bucket(addPhoto.PhotoBytes,buckets.FirstOrDefault(x=>x.BucketName=="TourImages")!.BucketId.ToString(),addPhoto.PhotoName))
                {
                    var photo =  await _s3Service.GetImageFromS3Bucket(addPhoto.PhotoName,buckets.FirstOrDefault(x=>x.BucketName=="TourImages")!.BucketId.ToString());
                    var result = await _unitOfWork.Photos.AddAsync(photo);
                    if(_unitOfWork.Save()>=0)
                    {
                        _logger.LogDebug("Successefully added photo");
                        return result.Id;
                    }
                    throw new DatabaseException("Error saving photo");
            
                }
                throw new UploadImageException("Cannot upload image to s3 bucket");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error adding photo: {errorMessage}", ex.Message);
                throw new DatabaseException("Error adding photo", ex);
            }
        }

        public async Task<PhotoDto> GetPhoto(GetPhotoRequest getPhoto)
        {
            try
            {
                Photo currentPhoto = await _unitOfWork.Photos.FindOneAsync(x=>x.Id == getPhoto.PhotoId);
                return _mapper.Map<PhotoDto>(currentPhoto);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting photo: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting photo", ex);
            }
        }
        public IQueryable<PhotoDto> GetPhotos(GetPhotosRequest getPhotos)
        {
            try
            {
               return _mapper.ProjectTo<PhotoDto>(_unitOfWork.Photos.GetAll().Where(x=>x.TourId == getPhotos.TourId));
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting photos: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting photos", ex);
            }
        }
        public bool RemovePhoto(RemovePhotoRequest removePhoto)
        {
            try
            {
                Photo currentPhoto = _unitOfWork.Photos.FindOneAsync(x=>x.Id == removePhoto.PhotoId).Result;
                List<Bucket> buckets = _configuration.GetSection("Buckets").Get<List<Bucket>>() ?? throw new NullReferenceException();
               
                if(_s3Service.DeleteImageFromS3Bucket(currentPhoto.Title, buckets.FirstOrDefault(x=>x.BucketName=="TourImages")!.BucketId.ToString()).Result)
                {
                    _unitOfWork.Photos.Delete(currentPhoto);
                     if(_unitOfWork.Save()>=0)
                    {
                        _logger.LogDebug("Successefully removed payment method!");
                        return true;
                    }
                    throw new DatabaseException("Removing payment method went wrong!");
                }
                throw new DeleteImageException("Error deleting image from s3 bucket");
               
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error removing photo: {errorMessage}", ex.Message);
                throw new DatabaseException("Error removing photo", ex);
            }
        }

        public bool UpdatePhoto(UpdatePhotoRequest updatePhoto)
        {
            try
            {
                var currentPhoto = _unitOfWork.Photos.FindOneAsync(x=>x.Id == updatePhoto.PhotoId).Result;
                currentPhoto.Title = updatePhoto.PhotoName;
                List<Bucket> buckets = _configuration.GetSection("Buckets").Get<List<Bucket>>() ?? throw new NullReferenceException();
                
                if(updatePhoto.PhotoBytes != null)
                {
                    if(_s3Service.DeleteImageFromS3Bucket(currentPhoto.Title,buckets.FirstOrDefault(x=>x.BucketName=="TourImages")!.BucketId.ToString()).Result)
                    {
                        if(_s3Service.UploadImageToS3Bucket(updatePhoto.PhotoBytes,currentPhoto.Title,buckets.FirstOrDefault(x=>x.BucketName=="TourImages")!.BucketId.ToString()).Result)
                        {
                            var newPhoto = _s3Service.GetImageFromS3Bucket(currentPhoto.Title,buckets.FirstOrDefault(x=>x.BucketName=="TourImages")!.BucketId.ToString()).Result;
                            currentPhoto.FileLink = newPhoto.FileLink;
                        }
                        _logger.LogError("Error uploading new image version");
                        throw new UploadImageException("Error uploading new image version");
                    }
                    _logger.LogError("Error deleting previos version of image");
                    throw new DeleteImageException("Error deleting previos version of image");
                }
                _unitOfWork.Photos.Update(currentPhoto);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully updated photo variant!");
                    return true;
                }
                throw new DatabaseException("Updating photo went wrong!");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error updating photo: {errorMessage}", ex.Message);
                throw new DatabaseException("Error updating photo", ex);
            }
        }

    }
}