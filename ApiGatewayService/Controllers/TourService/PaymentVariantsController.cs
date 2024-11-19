using ApiGatewayService.Models.TourService.Models.PaymentVariant.Requests;
using ApiGatewayService.Services.TourService.PaymentVariants;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;

namespace ApiGatewayService.Controllers.TourService;

[ApiController]
[Route("api/[controller]")]
public class PaymentVariantsController(ILogger<PaymentMethodController> logger, ITourPaymentVariantsService tourPaymentVariantsService) : Controller
{
    private readonly ILogger<PaymentMethodController> _logger = logger;
    private readonly ITourPaymentVariantsService _tourPaymentVariantsService = tourPaymentVariantsService;

    [HttpPost]
    [Route("paymentVariants")]
    public async Task<IActionResult> AddPaymentVariant(AddPaymentVariantRequest request)
    {
        try
        {
            var result = await _tourPaymentVariantsService.AddPaymentVariant(request);
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
    [HttpGet]
    [Route("paymentVariants/{paymentVariantId}")]
    public async Task<IActionResult> GetPaymentVariant([FromRoute] long paymentVariantId)
    {
        GetPaymentVariantRequest request = new() {PaymentVariantId = paymentVariantId};
        try
        {
            var result = await _tourPaymentVariantsService.GetPaymentVariant(request);
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
    [HttpGet]
    [Route("paymentVariants/paymentMethod/{paymentMethodId}")]
    public async Task<IActionResult> GetPaymentVariants([FromRoute] long paymentMethodId)
    {
        GetPaymentVariantsRequest request = new() {PaymentMethodId = paymentMethodId};
        try
        {
            var result = await _tourPaymentVariantsService.GetPaymentVariants(request);
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
    [HttpPut]
    [Route("paymentVariants")]
    public async Task<IActionResult> UpdatePaymentVariant(UpdatePaymentVariantRequest request)
    {
        try
        {
            var result = await _tourPaymentVariantsService.UpdatePaymentVariant(request);
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
    [HttpDelete]
    [Route("paymentVariants")]
    public async Task<IActionResult> RemovePaymentVariant(RemovePaymentVariantRequest request)
    {
        try
        {
            var result = await _tourPaymentVariantsService.RemovePaymentVariant(request);
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