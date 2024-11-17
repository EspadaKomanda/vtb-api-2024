using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGatewayService.Models.TourService.Models.Category.Requests;
using ApiGatewayService.Models.TourService.Models.Category.Responses;

namespace ApiGatewayService.Services.TourService.Categories
{
    public interface ITourCategoryService
    {
        Task<AddCategoryResponse> AddCategory(AddCategoryRequest request);
        Task<UpdateCategoryResponse> UpdateCategory(UpdateCategoryRequest request);
        Task<GetCategoriesResponse> GetCategories(GetCategoriesRequest request);
        Task<GetCategoryResponse> GetCategory(GetCategoryRequest request);
        Task<RemoveCategoryResponse> RemoveCategory(RemoveCategoryRequest request);
    }
}