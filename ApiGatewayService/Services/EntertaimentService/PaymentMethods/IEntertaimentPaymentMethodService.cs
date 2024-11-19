using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Models.PaymentMethod.Requests;
using EntertaimentService.Models.PaymentMethod.Responses;

namespace ApiGatewayService.Services.EntertaimentService.PaymentMethods
{
    public interface IEntertaimentPaymentMethodService
    {
        Task<AddPaymendMethodEntertainmentResponse> AddPaymentMethod(AddPaymentMethodEntertaimentRequest request);
        Task<UpdatePaymentMethodEntertainmentResponse> UpdatePaymentMethod(UpdatePaymentMethodEntertaimentRequest request);
        Task<GetPaymentMethodEntertaimentResponse> GetPaymentMethod(GetPaymentMethodEntertainmentRequest request);
        Task<RemovePaymentMethodEntertaimentResponse> RemovePaymentMethod(RemovePaymentMethodEntertaimentRequest request);
    }
}