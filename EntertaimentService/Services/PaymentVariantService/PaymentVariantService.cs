using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntertaimentService.Database.Models;
using EntertaimentService.Exceptions.Database;
using EntertaimentService.Models.PaymentVariant;
using EntertaimentService.Models.PaymentVariant.Requests;
using UserService.Repositories;

namespace EntertaimentService.Services.PaymentVariantService
{
    public class PaymentVariantService(IUnitOfWork unitOfWork, ILogger<PaymentVariantService> logger, IMapper mapper) : IPaymentVariantService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<PaymentVariantService> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public async Task<long> AddPaymentVariant(PaymentVariant paymentVariant)
        {
            try
            {
                var result = await _unitOfWork.PaymentVariants.AddAsync(paymentVariant);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully added payment variant");
                    return result.Id;
                }
                throw new DatabaseException("Error saving payment variant");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error adding payment variant: {errorMessage}", ex.Message);
                throw new DatabaseException("Error adding payment variant", ex);
            }
        }

        public async Task<PaymentVariantDto> GetPaymentVariant(GetPaymentVariantRequest getPaymentVariant)
        {
            //TODO: setup autommapper for PaymentVariant
            try
            {
                PaymentVariant currentPaymentVariant = await _unitOfWork.PaymentVariants.FindOneAsync(x=>x.Id == getPaymentVariant.PaymentVariantId);
                return _mapper.Map<PaymentVariantDto>(currentPaymentVariant);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting payment variant: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting payment variant", ex);
            }
        }
        public IQueryable<PaymentVariantDto> GetPaymentVariants(GetPaymentVariantsRequest getPaymentVariants)
        {
            //TODO: setup autommapper for PaymentVariant
            try
            {
               return _mapper.ProjectTo<PaymentVariantDto>(_unitOfWork.PaymentVariants.GetAll().Where(x=>x.PaymentMethodId == getPaymentVariants.PaymentMethodId));
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting payment variants: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting payment variants", ex);
            }
        }
        public bool RemovePaymentVariant(RemovePaymentVariantRequest removePaymentVariant)
        {
            try
            {
                _unitOfWork.PaymentVariants.Delete( _unitOfWork.PaymentVariants.FindOneAsync(x=>x.Id == removePaymentVariant.PaymentVariantId).Result);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully removed payment method!");
                    return true;
                }
                throw new DatabaseException("Removing payment method went wrong!");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error removing payment method: {errorMessage}", ex.Message);
                throw new DatabaseException("Error removing payment method", ex);
            }
        }

        public bool UpdatePaymentVariant(UpdatePaymentVariantRequest updatePaymentVariant)
        {
            try
            {
                var currentPaymentVariant = _unitOfWork.PaymentVariants.FindOneAsync(x=>x.Id == updatePaymentVariant.PaymentVariantId).Result;
                currentPaymentVariant.Name = updatePaymentVariant.PaymentVariantName;
                currentPaymentVariant.Price = updatePaymentVariant.Price;
                currentPaymentVariant.PaymentMethodId = updatePaymentVariant.PaymentMethodId;
                _unitOfWork.PaymentVariants.Update(currentPaymentVariant);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully updated payment variant!");
                    return true;
                }
                throw new DatabaseException("Updating payment variant went wrong!");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error updating payment variant: {errorMessage}", ex.Message);
                throw new DatabaseException("Error updating payment variant", ex);
            }
        }
    }
}