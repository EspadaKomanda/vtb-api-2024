using ApiGatewayService.Controllers.TourService;
using ApiGatewayService.Services.EntertaimentService.PaymentVariants;
using EntertaimentService.Models.PaymentVariant.Requests;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;

namespace ApiGatewayService.Controllers.EntertainmentService;

[ApiController]
[Route("api/[controller]/ent")]
public class PaymentVariantsController(ILogger<PaymentMethodController> logger, IEntertaimentPaymentVariant entertainmentPaymentVariantsService) : Controller
{
    private readonly ILogger<PaymentMethodController> _logger = logger;
    private readonly IEntertaimentPaymentVariant _entertainmentPaymentVariantsService = entertainmentPaymentVariantsService;

    [HttpPost]
    [Route("paymentVariants")]
    public async Task<IActionResult> AddPaymentVariant(AddPaymentVariantEntertaimentRequest request)
    {
        try
        {
            var result = await _entertainmentPaymentVariantsService.AddPaymentVariant(request);
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
        GetPaymentVariantEntertainmentRequest request = new() {PaymentVariantId = paymentVariantId};
        try
        {
            var result = await _entertainmentPaymentVariantsService.GetPaymentVariant(request);
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
        GetPaymentVariantsEntertainmentRequest request = new() {PaymentMethodId = paymentMethodId};
        try
        {
            var result = await _entertainmentPaymentVariantsService.GetPaymentVariants(request);
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
    public async Task<IActionResult> UpdatePaymentVariant(UpdatePaymentVariantEntertaimentRequest request)
    {
        try
        {
            var result = await _entertainmentPaymentVariantsService.UpdatePaymentVariant(request);
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
    public async Task<IActionResult> RemovePaymentVariant(RemovePaymentVariantEntertainmentRequest request)
    {
        try
        {
            var result = await _entertainmentPaymentVariantsService.RemovePaymentVariant(request);
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