using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Models.Category.Requests;
using EntertaimentService.Models.Category.Responses;

namespace ApiGatewayService.Services.EntertaimentService.Categories
{
    public interface IEntertaimentCategoryService
    {
        Task<AddCategoryResponse> AddCategory(AddCategoryRequest request);
        Task<UpdateCategoryResponse> UpdateCategory(UpdateCategoryRequest request);
        Task<GetCategoriesResponse> GetCategories(GetCategoriesRequest request);
        Task<GetCategoryResponse> GetCategory(GetCategoryRequest request);
        Task<RemoveCategoryResponse> RemoveCategory(RemoveCategoryRequest request);
    }
}