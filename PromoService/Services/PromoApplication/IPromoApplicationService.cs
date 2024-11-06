namespace PromoService.Services.PromoApplication;

public interface IPromoApplicationService
{
    /// <summary>
    /// Фиксирует факт использования пользователем промокода для определенного набора товаров
    /// </summary>
    Task RegisterPromoUse();

    /// <summary>
    /// Позволяет удостовериться, что пользователь может применить промокод для данного набора товаров
    /// </summary>
    Task ValidatePromocodeApplication();

    /// <summary>
    /// Позволяет получить промокоды, использованные пользователем, и информацию о них
    /// </summary>
    Task GetMyPromoApplications();
}