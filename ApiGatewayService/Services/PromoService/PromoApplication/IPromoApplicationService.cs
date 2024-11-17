
using ApiGatewayService.Models.PromoService.PromoApplication.Requests;
using ApiGatewayService.Models.PromoService.PromoApplication.Responses;

namespace ApiGatewayService.Services.PromoService.PromoApplication;

public interface IPromoApplicationService
{
    /// <summary>
    /// Фиксирует факт использования пользователем промокода для определенного набора товаров
    /// </summary>
    Task<RegisterPromoUseResponse> RegisterPromoUse(RegisterPromoUseRequest request);

    /// <summary>
    /// Позволяет удостовериться, что пользователь может применить промокод для данного набора товаров
    /// </summary>
    Task<ValidatePromocodeApplicationResponse> ValidatePromocodeApplication(ValidatePromocodeApplicationRequest request);

    /// <summary>
/// Позволяет получить промокоды, использованные пользователем, и информацию о них
    /// </summary>
    Task<GetMyPromoApplicationsResponse> GetMyPromoApplications(GetMyPromoApplicationsRequest request);
}