using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TourService.Database.Models;
using TourService.Exceptions.Database;
using TourService.Models.Benefits;
using UserService.Repositories;

namespace TourService.Services.BenefitService
{
    public class BenefitService(IUnitOfWork unitOfWork, ILogger<BenefitService> logger, IMapper mapper) : IBenefitService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<BenefitService> _logger = logger;
        private readonly IMapper _mapper = mapper;
        public async Task<long> AddBenefit(Benefit benefit)
        {
            try
            {
                var result = await _unitOfWork.Benefits.AddAsync(benefit);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully added benefit");
                    return result.Id;
                }
                throw new DatabaseException("Error saving benefit");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error adding benefit: {errorMessage}", ex.Message);
                throw new DatabaseException("Error adding benefit", ex);
            }
        }

        public async Task<BenefitDto> GetBenefit(GetBenefitRequest getBenefit)
        {
            try
            {
                Benefit currentBenefit = await _unitOfWork.Benefits.FindOneAsync(x=>x.Id == getBenefit.BenefitId);
                return _mapper.Map<BenefitDto>(currentBenefit);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting benefit: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting benefit", ex);
            }
        }

        public IQueryable<BenefitDto> GetBenefits(GetBenefitsRequest getBenefits)
        {
            try
            {
                var currentBenefits = _unitOfWork.Benefits.GetAll().Skip((getBenefits.Page - 1) * 10).Take(10);
                return _mapper.ProjectTo<BenefitDto>(currentBenefits);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting benefits: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting benefits", ex);
            }
        }

        public bool RemoveBenefit(RemoveBenefitRequest removeBenefit)
        {
            try
            {
                
                _unitOfWork.ReviewBenefits.DeleteMany(x=>x.Id==removeBenefit.BenefitId);
                _unitOfWork.Benefits.Delete( _unitOfWork.Benefits.FindOneAsync(x=>x.Id == removeBenefit.BenefitId).Result);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully removed benefit!");
                    return true;
                }
                throw new DatabaseException("Removing benefit went wrong!");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error removing benefit: {errorMessage}", ex.Message);
                throw new DatabaseException("Error removing benefit", ex);
            }
        }

        public bool UpdateBenefit(UpdateBenefitRequest updateBenefit)
        {
            try
            {
                var currentBenefit = _unitOfWork.Benefits.FindOneAsync(x=>x.Id == updateBenefit.BenefitId).Result;
                currentBenefit.Name = updateBenefit.BenefitName;

                _unitOfWork.Benefits.Update(currentBenefit);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully updated benefit!");
                    return true;
                }
                throw new DatabaseException("Updating benefit went wrong!");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error updating benefit: {errorMessage}", ex.Message);
                throw new DatabaseException("Error updating benefit", ex);
            }
        }
    }
}