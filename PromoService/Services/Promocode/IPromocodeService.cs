namespace PromoService.Services.Promocode;

public interface IPromocodeService
{
    /// <summary>
    /// Добавляет промокод с его настройками.
    /// </summary>
    Task CreatePromocode();

    /// <summary>
    /// Деактивирует промокод (промокод остается в базе данных).
    /// </summary>
    Task DeletePromocode();

    /// <summary>
    /// Редактирует настройки промокода.
    /// </summary>
    Task SetPromoMeta();

    /// <summary>
    /// Устанавливает ограничения промокода.
    /// </summary>
    Task SetPromoRestrictions();
}