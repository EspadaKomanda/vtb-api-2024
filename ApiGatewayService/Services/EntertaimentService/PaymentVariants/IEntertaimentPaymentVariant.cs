using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Models.PaymentVariant.Requests;
using EntertaimentService.Models.PaymentVariant.Responses;

namespace ApiGatewayService.Services.EntertaimentService.PaymentVariants
{
    public interface IEntertaimentPaymentVariant
    {
        Task<AddPaymentVariantResponse> AddPaymentVariant(AddPaymentVariantEntertaimentRequest request);
        Task<GetPaymentVariantsResponse> GetPaymentVariants(GetPaymentVariantsEntertainmentRequest request);
        Task<UpdatePaymentVariantResponse> UpdatePaymentVariant(UpdatePaymentVariantEntertaimentRequest request);
        Task<RemovePaymentVariantResponse> RemovePaymentVariant(RemovePaymentVariantEntertainmentRequest request);
        Task<GetPaymentVariantResponse> GetPaymentVariant(GetPaymentVariantEntertainmentRequest request);
    }
}