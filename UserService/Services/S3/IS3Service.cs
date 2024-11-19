using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Database.Models;

namespace EntertaimentService.Services.S3
{
    public interface IS3Service
    {
        //FIXME: Fix responses
        Task ConfigureBuckets();
        Task<bool> UploadImageToS3Bucket(byte[] imageBytes, string template, string imageName);
        Task<bool> DeleteImageFromS3Bucket(string fileName, string bucketName);
        Task<Photo> GetImageFromS3Bucket(string fileName, string bucketName);
        Task<bool> CheckIfBucketExists(string bucketName);
        Task<bool> DeleteBucket(string bucketName);
        Task<bool> CreateBucket(string bucketName);
    }
}