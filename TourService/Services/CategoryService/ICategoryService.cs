using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourService.Database.Models;
using TourService.Models.Category;
using TourService.Models.Category.Requests;

namespace TourService.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<long> AddCategory(Category category);
        Task<CategoryDto> GetCategory(GetCategoryRequest getCategory);
        IQueryable<CategoryDto> GetCategories(GetCategoriesRequest getCategories);
        bool RemoveCategory(RemoveCategoryRequest removeCategory);
        bool UpdateCategory(UpdateCategoryRequest updateCategory);
    }
}