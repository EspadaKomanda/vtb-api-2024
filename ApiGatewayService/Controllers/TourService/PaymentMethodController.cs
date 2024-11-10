using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayService.Controllers.TourService;

[ApiController]
[Route("api/[controller]")]
public class PaymentMethodController : ControllerBase
{
    public Task<IActionResult> GetPaymentMethod()
    {
        throw new NotImplementedException();
    }
}