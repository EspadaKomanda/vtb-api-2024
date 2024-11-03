using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using ImageAgregationService.Exceptions.S3ServiceExceptions;
using TourService.Database.Models;
using TourService.Exceptions.S3ServiceExceptions;
using TourService.Utils.Models;

namespace TourService.Services.S3
{
     public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly ILogger<S3Service> _logger;
        private readonly IConfiguration _configuration;
        public S3Service(IAmazonS3 s3Client, ILogger<S3Service> logger, IConfiguration configuration)
        {
            _s3Client = s3Client;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task ConfigureBuckets()
        {
            try
            {
               
                List<Bucket> buckets = _configuration.GetSection("Buckets").Get<List<Bucket>>() ?? throw new NullReferenceException();
                foreach (var bucket in buckets)
                {
                    
                    if(!await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client,bucket.BucketId.ToString()))
                    {
                        await _s3Client.PutBucketAsync(bucket.BucketId.ToString());
                    }   
                }
                
                foreach(var bucket in buckets)
                {
                    if(!await CheckIfBucketExists(bucket.BucketId.ToString()))
                    {
                        _logger.LogError("Failed to configure S3 buckets, storage unavailable!");
                        throw new StorageUnavailibleException("Failed to configure S3 buckets, storage unavailable!");
                    }   
                }
                _logger.LogInformation("S3 buckets configured!");
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Failed to configure S3 buckets!");
                throw new ConfigureBucketsException("Failed to configure S3 buckets!", ex);
            }
        }
        public async Task<bool> CreateBucket(string bucketName)
        {
            try
            {
                if(!await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client,bucketName))
                {
                    await _s3Client.PutBucketAsync(bucketName);
                    return true;
                }   
                _logger.LogInformation($"Bucket {bucketName} already exists!");
                throw new BucketAlreadyExistsException($"Bucket {bucketName} already exists!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create bucket!");
                throw new ConfigureBucketsException("Failed to create bucket!", ex);
            }
        }
       
        public async Task<bool> CheckIfBucketExists(string bucketName)
        {
            return await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
        }
        public async Task<bool> DeleteBucket(string bucketName)
        {
            try
            {
                DeleteBucketResponse response = await _s3Client.DeleteBucketAsync(bucketName);
                _logger.LogInformation(response.HttpStatusCode.ToString()+ response.ResponseMetadata.ToString());
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"Bucket {bucketName} deleted!");
                    return true;
                }
                if (response.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogError($"Bucket {bucketName} not found!");
                    throw new BucketNotFoundException($"Bucket {bucketName} not found!");
                }
                if (response.HttpStatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    _logger.LogError($"Failed to delete bucket {bucketName}!");
                    throw new DeleteBucketException($"Failed to delete bucket {bucketName}!");
                }
                _logger.LogError($"Failed to delete bucket {bucketName}, unhandled exception!");
                throw new DeleteBucketException($"Failed to delete bucket {bucketName}, unhandled exception!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete bucket {bucketName}!");
                throw new DeleteBucketException($"Failed to delete bucket {bucketName}!", ex);
            }
        }
        public async Task<bool> DeleteImageFromS3Bucket(string fileName, string bucketName)
        {
            try
            {
                DeleteObjectResponse response = await _s3Client.DeleteObjectAsync(bucketName, fileName);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"Image {fileName} deleted from S3 bucket {bucketName}!");
                    return true;
                }
                if (response.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogError($"Image {fileName} not found in S3 bucket {bucketName}!");
                    throw new ImageNotFoundException($"Image {fileName} not found in S3 bucket {bucketName}!");
                }
                if (response.HttpStatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    _logger.LogError($"Failed to delete image {fileName} from S3 bucket {bucketName}, storage unavailible!");
                    throw new DeleteImageException($"Failed to delete image {fileName} from S3 bucket {bucketName}, storage unavailible!");
                }
                _logger.LogError($"Failed to delete image {fileName} from S3 bucket {bucketName}, unhandled exception!" + response.ToString());
                throw new DeleteImageException($"Failed to delete image {fileName} from S3 bucket {bucketName}!, unhandled exception!" + response.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete image from S3 bucket!");
                throw new DeleteImageException("Failed to delete image from S3 bucket!", ex);
            }
        }

        public async Task<Photo> GetImageFromS3Bucket(string fileName, string bucketName)
        {
            try
            {
                GetObjectMetadataResponse metadataResponse = await _s3Client.GetObjectMetadataAsync(bucketName, fileName);
                if(metadataResponse.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    var response =  _s3Client.GetPreSignedURL(new GetPreSignedUrlRequest(){ BucketName = bucketName, Key = fileName, Expires = DateTime.Now.AddYears(10), Protocol = Protocol.HTTP});
                    _logger.LogInformation($"Uri for image {fileName} aquired from S3 bucket {bucketName}!");
                    return new Photo()
                    {
                       FileLink = response
                       
                    };
                }
                if(metadataResponse.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogError($"Image {fileName} not found in S3 bucket {bucketName}!");
                    throw new ImageNotFoundException($"Image {fileName} not found in S3 bucket {bucketName}!");
                }
                if(metadataResponse.HttpStatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    _logger.LogError($"Failed to get image {fileName} from S3 bucket {bucketName}, storage unavailible!");
                    throw new GetImageException($"Failed to get image {fileName} from S3 bucket {bucketName}, storage unavailible!");
                }
                _logger.LogError($"Failed to get image {fileName} from S3 bucket {bucketName}, unhandled exception!" + metadataResponse.ToString());
                throw new GetImageException($"Failed to get image {fileName} from S3 bucket {bucketName}!, unhandled exception!" + metadataResponse.ToString());
                                    
            }      
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get image from S3 bucket!");
                throw new GetImageException("Failed to get image from S3 bucket!", ex);
            }
        }
        public async Task<bool> UploadImageToS3Bucket(byte[] imageBytes, string template, string imageName)
        {
            try
            {
                PutObjectResponse response = await _s3Client.PutObjectAsync(new PutObjectRequest
                {
                    BucketName = template,
                    Key = imageName,
                    InputStream = new MemoryStream(imageBytes),
                    ContentType = "image/png"
                });
                if(response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"Image {imageName} uploaded to S3 bucket {template}!");
                    return true;
                }
                if(response.HttpStatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    _logger.LogError($"Failed to upload image {imageName} to S3 bucket {template}, storage unavailable!");
                    throw new StorageUnavailibleException("Failed to upload image to S3 bucket, storage unavailable!");
                }
                _logger.LogError($"Failed to upload image {imageName} to S3 bucket {template}, unhandled exception!" + response.ToString());
                throw new UploadImageException($"Failed to upload image {imageName} to S3 bucket {template}, unhandled exception!" + response.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload image to S3 bucket!");
                throw new UploadImageException("Failed to upload image to S3 bucket!", ex);
            }
        }
    }
}