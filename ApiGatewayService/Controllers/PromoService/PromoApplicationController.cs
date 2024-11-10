using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayService.Controllers.PromoService;

[ApiController]
[Route("api/[controller]")]
public class PromoApplicationController : ControllerBase
{
    /// <summary>
    /// Фиксирует факт использования пользователем промокода для определенного набора товаров
    /// </summary>
    Task<IActionResult> RegisterPromoUse(long userId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Позволяет удостовериться, что пользователь может применить промокод для данного набора товаров
    /// </summary>
    Task<IActionResult> ValidatePromocodeApplication(long userId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Позволяет получить промокоды, использованные пользователем, и информацию о них
    /// </summary>
    Task<IActionResult> GetMyPromoApplications(long userId)
    {
        throw new NotImplementedException();
    }
}