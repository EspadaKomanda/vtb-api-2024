using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Database.Models;
using EntertaimentService.Models.Category;
using EntertaimentService.Models.Category.Requests;
using TourService.Models.Category;
using TourService.Models.Category.Requests;

namespace EntertaimentService.Services.CategoryService
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