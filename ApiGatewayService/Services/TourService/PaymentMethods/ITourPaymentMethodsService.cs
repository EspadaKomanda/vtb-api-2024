using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGatewayService.Models.TourService.Models.PaymentMethod.Requests;
using ApiGatewayService.Models.TourService.Models.PaymentMethod.Responses;

namespace ApiGatewayService.Services.TourService.PaymentMethods
{
    public interface ITourPaymentMethodsService
    {
        Task<AddPaymendMethodResponse> AddPaymentMethod(AddPaymentMethodRequest request);
        Task<UpdatePaymentMethodResponse> UpdatePaymentMethod(UpdatePaymentMethodRequest request);
        Task<GetPaymentMethodResponse> GetPaymentMethod(GetPaymentMethodRequest request);
        Task<RemovePaymentMethodResponse> RemovePaymentMethod(RemovePaymentMethodRequest request);
    }
}