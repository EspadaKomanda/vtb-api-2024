using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EntertaimentService.Database.Models;
using EntertaimentService.Exceptions.Database;
using EntertaimentService.Models.PaymentMethod.Requests;
using EntertaimentService.Services.PaymentMethodService;
using TourService.Models.PaymentMethod;
using UserService.Repositories;

namespace TourService.Services.PaymentMethodService
{
    public class PaymentMethodService(IUnitOfWork unitOfWork, ILogger<PaymentMethodService> logger, IMapper mapper) : IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<PaymentMethodService> _logger = logger;
        private readonly IMapper _mapper = mapper;
        public async Task<long> AddPaymentMethod(PaymentMethod paymentMethod)
        {
            try
            {
                var result = await _unitOfWork.PaymentMethods.AddAsync(paymentMethod);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully added payment method");
                    return result.Id;
                }
                throw new DatabaseException("Error saving payment method");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error adding payment method: {errorMessage}", ex.Message);
                throw new DatabaseException("Error adding payment method", ex);
            }
        }

        public async Task<PaymentMethodDto> GetPaymentMethod(GetPaymentMethodRequest getPaymentMethod)
        {
            //TODO: setup autommapper for PaymentMethod
            try
            {
                PaymentMethod currentPaymentMethod = await _unitOfWork.PaymentMethods.FindOneAsync(x=>x.Id == getPaymentMethod.PaymentMethodId);
                return _mapper.Map<PaymentMethodDto>(currentPaymentMethod);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error getting payment method: {errorMessage}", ex.Message);
                throw new DatabaseException("Error getting payment method", ex);
            }
        }

        public bool RemovePaymentMethod(RemovePaymentMethodRequest removePaymentMethod)
        {
            try
            {
                _unitOfWork.PaymentMethods.Delete( _unitOfWork.PaymentMethods.FindOneAsync(x=>x.Id == removePaymentMethod.PaymentMethodId).Result);
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

        public bool UpdatePaymentMethod(UpdatePaymentMethodRequest updatePaymentMethod)
        {
            try
            {
                var currentPaymentMethod = _unitOfWork.PaymentMethods.FindOneAsync(x=>x.Id == updatePaymentMethod.PaymentMethodId).Result;
                currentPaymentMethod.Name = updatePaymentMethod.PaymentMethodName;
                _unitOfWork.PaymentMethods.Update(currentPaymentMethod);
                if(_unitOfWork.Save()>=0)
                {
                    _logger.LogDebug("Successefully updated payment method!");
                    return true;
                }
                throw new DatabaseException("Updating payment method went wrong!");
            }
            catch(Exception ex)
            {
                if(ex is DatabaseException)
                {
                    throw;
                }
                _logger.LogError("Error updating payment method: {errorMessage}", ex.Message);
                throw new DatabaseException("Error updating payment method", ex);
            }
        }
 
    }
}