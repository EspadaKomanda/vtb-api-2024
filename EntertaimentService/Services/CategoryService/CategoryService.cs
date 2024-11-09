using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntertaimentService.Database.Models;
using EntertaimentService.Exceptions.Database;
using EntertaimentService.Models.Category;
using EntertaimentService.Models.Category.Requests;
using UserService.Repositories;

namespace EntertaimentService.Services.CategoryService
{
    public class CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger, IMapper mapper) : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<CategoryService> _logger = logger;
        private readonly IMapper _mapper = mapper;
        public async Task<long> AddCategory(Category category)
        {
            try
            {
                var result = await _unitOfWork.Categories.AddAsync(category);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully added category");
                    return result.Id;
                }
                throw new DatabaseException("Error saving category");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error adding category: {errorMessage}", ex.Message);
                throw new DatabaseException("Error adding category", ex);
            }
        }

        public async Task<CategoryDto> GetCategory(GetCategoryRequest getCategory)
        {
            try
            {
                Category currentCategory = await _unitOfWork.Categories.FindOneAsync(x=>x.Id == getCategory.CategoryId);
                return _mapper.Map<CategoryDto>(currentCategory);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting category: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting category", ex);
            }
        }

        public IQueryable<CategoryDto> GetCategories(GetCategoriesRequest getCategories)
        {
            try
            {
                var currentBenefits = _unitOfWork.Categories.GetAll().Skip((getCategories.Page - 1) * 10).Take(10);
                return _mapper.ProjectTo<CategoryDto>(currentBenefits);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting benefits: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting benefits", ex);
            }
        }

        public bool RemoveCategory(RemoveCategoryRequest removeCategory)
        {
            using var transaction = _unitOfWork.BeginTransaction();
            try
            {
                 _unitOfWork.Categories.Delete( _unitOfWork.Categories.FindOneAsync(x=>x.Id == removeCategory.CategoryId).Result);
                _unitOfWork.EntertaimentCategories.DeleteMany(x=>x.CategoryId==removeCategory.CategoryId);
                if(transaction.SaveAndCommit())
                {
                    _logger.LogDebug("Successefully removed category!");
                    return true;
                }
                throw new DatabaseException("Removing category went wrong!");
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error removing category: {errorMessage}", ex.Message);
                throw new DatabaseException("Error removing category", ex);
            }
        }

        public bool UpdateCategory(UpdateCategoryRequest updateCategory)
        {
            try
            {
                var currentCategory = _unitOfWork.Categories.FindOneAsync(x=>x.Id == updateCategory.CategoryId).Result;
                currentCategory.Name = updateCategory.CategoryName;

                _unitOfWork.Categories.Update(currentCategory);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully updated category!");
                    return true;
                }
                throw new DatabaseException("Updating category went wrong!");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error updating category: {errorMessage}", ex.Message);
                throw new DatabaseException("Error updating category", ex);
            }
        }
 
    }
}