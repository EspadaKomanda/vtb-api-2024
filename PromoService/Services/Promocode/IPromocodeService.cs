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
    /// Получить все существующие промокоды или определенный промокод.
    /// </summary>
    GetPromocodesResponse GetPromocodes(GetPromocodesRequest request);
    /// <summary>
    /// Получить активные промокоды.
    /// </summary>
    GetActivePromocodesResponse GetActivePromocodes(GetActivePromocodesRequest request);
}