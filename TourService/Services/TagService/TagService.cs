using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TourService.Database.Models;
using TourService.Exceptions.Database;
using TourService.Models.DTO;
using TourService.Models.Tag.Requests;
using UserService.Repositories;

namespace TourService.Services.TagService
{
    public class TagService(ILogger<TagService> logger, IUnitOfWork unitOfWork, IMapper mapper) : ITagService
    {
        private readonly ILogger<TagService> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<long> AddTag(Tag tag)
        {
            try
            {
                var result = await _unitOfWork.Tags.AddAsync(tag);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully added tag");
                    return result.Id;
                }
                throw new DatabaseException("Error saving tag");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error adding tag: {errorMessage}", ex.Message);
                throw new DatabaseException("Error adding tag", ex);
            }
        }
        public async Task<TagDto> GetTag(GetTagRequest getTag)
        {
            try
            {
                Tag currentTag = await _unitOfWork.Tags.FindOneAsync(x=>x.Id == getTag.TagId);
                return _mapper.Map<TagDto>(currentTag);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting tag: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting tag", ex);
            }
        }
        public IQueryable<TagDto> GetTags(GetTagsRequest getTags)
        {
            try
            {
                var currentTags = _unitOfWork.Tags.GetAll().Skip((getTags.Page - 1) * 10).Take(10);
                return _mapper.ProjectTo<TagDto>(currentTags);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting tags: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting tags", ex);
            }
        }
        public bool RemoveTag(RemoveTagRequest removeTag)
        {
            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                _unitOfWork.Tags.Delete( _unitOfWork.Tags.FindOneAsync(x=>x.Id == removeTag.TagId).Result);
                _unitOfWork.TourTags.DeleteMany(x=>x.TagId==removeTag.TagId);
                if(transaction.SaveAndCommit())
                {
                    _logger.LogDebug("Successefully removed tag!");
                    return true;
                }
                throw new DatabaseException("Removing tag went wrong!");
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error removing tag: {errorMessage}", ex.Message);
                throw new DatabaseException("Error removing tag", ex);
            }
        }
        public bool UpdateTag(UpdateTagRequest updateTag)
        {
            try
            {
                var currentTag = _unitOfWork.Tags.FindOneAsync(x=>x.Id == updateTag.TagId).Result;
                currentTag.Name = updateTag.TagName;
                _unitOfWork.Tags.Update(currentTag);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully updated tag!");
                    return true;
                }
                throw new DatabaseException("Updating tag went wrong!");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error updating tag: {errorMessage}", ex.Message);
                throw new DatabaseException("Error updating tag", ex);
            }
        }
    }
}