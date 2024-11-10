using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourService.Database.Models;
using TourService.Models.PaymentMethod;
using TourService.Models.PaymentMethod.Requests;

namespace TourService.Services.PaymentMethodService
{
    public interface IPaymentMethodService
    {
        Task<long> AddPaymentMethod(PaymentMethod paymentMethod);
        Task<PaymentMethodDto> GetPaymentMethod(GetPaymentMethodRequest getPaymentMethod);
        bool UpdatePaymentMethod(UpdatePaymentMethodRequest updatePaymentMethod);
        bool RemovePaymentMethod(RemovePaymentMethodRequest removePaymentMethod);
    }
}