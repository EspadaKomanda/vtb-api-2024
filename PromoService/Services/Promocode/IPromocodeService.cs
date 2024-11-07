using PromoService.Models.Promocode.Requests;
using PromoService.Models.Promocode.Responses;

namespace PromoService.Services.Promocode;

public interface IPromocodeService
{
    /// <summary>
    /// Добавляет промокод с его настройками.
    /// </summary>
    Task<CreatePromocodeResponse> CreatePromocode(CreatePromocodeRequest request);

    /// <summary>
    /// Деактивирует промокод (промокод остается в базе данных).
    /// </summary>
    Task<DeletePromocodeResponse> DeletePromocode(DeletePromocodeRequest request);

    /// <summary>
    /// Редактирует настройки промокода.
    /// </summary>
    Task<SetPromocodeMetaResponse> SetPromocodeMeta(SetPromocodeMetaRequest request);

    /// <summary>
    /// Устанавливает ограничения промокода.
    /// </summary>
    Task<SetPromocodeRestrictionsResponse> SetPromocodeRestrictions(SetPromocodeRestrictionsRequest request);

    Task<GetPromocodesResponse> GetPromocodes(GetPromocodesRequest request);

    Task<GetActivePromocodesResponse> GetActivePromocodes(GetActivePromocodesRequest request);
}