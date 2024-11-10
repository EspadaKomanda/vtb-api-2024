using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourService.Database.Models;
using TourService.Models.PaymentVariant;
using TourService.Models.PaymentVariant.Requests;

namespace TourService.Services.PaymentVariantService
{
    public interface IPaymentVariantService
    {
        Task<long> AddPaymentVariant(PaymentVariant paymentVariant);
        Task<PaymentVariantDto> GetPaymentVariant(GetPaymentVariantRequest getPaymentVariant);
        IQueryable<PaymentVariantDto> GetPaymentVariants(GetPaymentVariantsRequest getPaymentVariants);
        bool UpdatePaymentVariant(UpdatePaymentVariantRequest updatePaymentVariant);
        bool RemovePaymentVariant(RemovePaymentVariantRequest removePaymentVariant);
    }
}