using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Database.Models;
using EntertaimentService.Models.PaymentMethod;
using EntertaimentService.Models.PaymentMethod.Requests;
using TourService.Models.PaymentMethod;

namespace EntertaimentService.Services.PaymentMethodService
{
    public interface IPaymentMethodService
    {
        Task<long> AddPaymentMethod(PaymentMethod paymentMethod);
        Task<PaymentMethodDto> GetPaymentMethod(GetPaymentMethodRequest getPaymentMethod);
        bool UpdatePaymentMethod(UpdatePaymentMethodRequest updatePaymentMethod);
        bool RemovePaymentMethod(RemovePaymentMethodRequest removePaymentMethod);
    }
}