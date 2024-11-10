using PromoService.Models.PromoApplication.Requests;
using PromoService.Models.PromoApplication.Responses;
using PromoService.Models.Promocode.Requests;

namespace PromoService.Services.PromoApplication;

public interface IPromoApplicationService
{
    /// <summary>
    /// Фиксирует факт использования пользователем промокода для определенного набора товаров
    /// </summary>
    Task<RegisterPromoUseResponse> RegisterPromoUse(long userId, RegisterPromoUseRequest request);

    /// <summary>
    /// Позволяет удостовериться, что пользователь может применить промокод для данного набора товаров
    /// </summary>
    Task<ValidatePromocodeApplicationResponse> ValidatePromocodeApplication(long userId, ValidatePromocodeApplicationRequest request);

    /// <summary>
    /// Позволяет получить промокоды, использованные пользователем, и информацию о них
    /// </summary>
    GetMyPromoApplicationsResponse GetMyPromoApplications(long userId, GetMyPromoApplicationsRequest request);
}