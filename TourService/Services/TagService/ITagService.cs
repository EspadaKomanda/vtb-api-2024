using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourService.Database.Models;
using TourService.Models.DTO;
using TourService.Models.Tag.Requests;

namespace TourService.Services.TagService
{
    public interface ITagService
    {
        Task<long> AddTag(Tag tag);
        Task<TagDto> GetTag(GetTagRequest getTag);
        IQueryable<TagDto> GetTags(GetTagsRequest getTags);
        bool RemoveTag(RemoveTagRequest removeTag);
        bool UpdateTag(UpdateTagRequest updateTag);
    }
}