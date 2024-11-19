using ApiGatewayService.Models.PromoService.PromoApplication.Requests;
using ApiGatewayService.Services.PromoService.PromoApplication;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;

namespace ApiGatewayService.Controllers.PromoService;

[ApiController]
[Route("api/[controller]")]
public class PromoApplicationController(ILogger<PromoApplicationController> logger, IPromoApplicationService promoappservice) : ControllerBase
{
    private readonly ILogger<PromoApplicationController> _logger = logger;
    private readonly IPromoApplicationService _promoAppService = promoappservice;

    /// <summary>
    /// Выполняет проверку промокода на валидность в заказе
    /// </summary>
    [HttpPost]
    [Route("promoApplications")]
    public async Task<IActionResult> RegisterPromoApplication(RegisterPromoUseRequest request)
    {
        try
        {
            var result = await _promoAppService.RegisterPromoUse(request);
            return Ok(result);
        }
        catch(Exception ex)
        {
            if(ex is MyKafkaException)
            {
                return StatusCode(500, ex.Message);
            }
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Выполняет проверку промокода на валидность в заказе
    /// </summary>
    [HttpPost]
    [Route("validateApplication")]
    public async Task<IActionResult> ValidatePromocodeApplication(ValidatePromocodeApplicationRequest request)
    {
        try
        {
            var result = await _promoAppService.ValidatePromocodeApplication(request);
            return Ok(result);
        }
        catch(Exception ex)
        {
            if(ex is MyKafkaException)
            {
                return StatusCode(500, ex.Message);
            }
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Позволяет получить промокоды, использованные пользователем, и информацию о них
    /// </summary>
    [HttpGet]
    [Route("promoApplications")]
    public IActionResult GetMyPromoApplications(GetMyPromoApplicationsRequest request)
    {
        try
        {
            var result = _promoAppService.GetMyPromoApplications(request);
            return Ok(result);
        }
        catch(Exception ex)
        {
            if(ex is MyKafkaException)
            {
                return StatusCode(500, ex.Message);
            }
            return BadRequest(ex.Message);
        }
    }
}