using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Database.Models;
using EntertaimentService.Models.PaymentVariant;
using EntertaimentService.Models.PaymentVariant.Requests;

namespace EntertaimentService.Services.PaymentVariantService
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