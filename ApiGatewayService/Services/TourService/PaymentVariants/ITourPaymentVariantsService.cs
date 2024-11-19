using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGatewayService.Models.TourService.Models.PaymentVariant.Requests;
using ApiGatewayService.Models.TourService.Models.PaymentVariant.Responses;

namespace ApiGatewayService.Services.TourService.PaymentVariants
{
    public interface ITourPaymentVariantsService
    {
        Task<AddPaymentVariantResponse> AddPaymentVariant(AddPaymentVariantRequest request);
        Task<GetPaymentVariantsResponse> GetPaymentVariants(GetPaymentVariantsRequest request);
        Task<UpdatePaymentVariantResponse> UpdatePaymentVariant(UpdatePaymentVariantRequest request);
        Task<RemovePaymentVariantResponse> RemovePaymentVariant(RemovePaymentVariantRequest request);
        Task<GetPaymentVariantResponse> GetPaymentVariant(GetPaymentVariantRequest request);
    }
}