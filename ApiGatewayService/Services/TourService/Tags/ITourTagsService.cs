using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGatewayService.Models.TourService.Models.Tag.Requests;
using ApiGatewayService.Models.TourService.Models.Tag.Responses;
using TourService.Models.Tag.Responses;

namespace ApiGatewayService.Services.TourService.Tags
{
    public interface ITourTagsService
    {
        Task<AddTagResponse> AddTag(AddTagRequest addTagRequest);
        Task<GetTagResponse> GetTag(GetTagRequest getTagRequest);
        Task<GetTagsResponse> GetTags(GetTagsRequest getTagsRequest);
        Task<RemoveTagResponse> RemoveTag(RemoveTagRequest removeTagRequest);
        Task<UpdateTagResponse> UpdateTag(UpdateTagRequest updateTagRequest);
    }
}