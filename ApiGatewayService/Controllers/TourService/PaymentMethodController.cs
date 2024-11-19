using ApiGatewayService.Models.TourService.Models.PaymentMethod.Requests;
using ApiGatewayService.Services.TourService.PaymentMethods;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;

namespace ApiGatewayService.Controllers.TourService;

[ApiController]
[Route("api/[controller]")]
public class PaymentMethodController(ILogger<PaymentMethodController> logger, ITourPaymentMethodsService tourPaymentMethodsService) : ControllerBase
{
    private readonly ILogger<PaymentMethodController> _logger = logger;
    private readonly ITourPaymentMethodsService _tourPaymentMethodsService = tourPaymentMethodsService;

    [HttpPost]
    [Route("paymentMethods")]
    public async Task<IActionResult> AddPaymentMethod(AddPaymentMethodRequest request)
    {
        try
        {
            var result = await _tourPaymentMethodsService.AddPaymentMethod(request);
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

    [HttpPatch]
    [Route("paymentMethods")]
    public async Task<IActionResult> UpdatePaymentMethod(UpdatePaymentMethodRequest request)
    {
        try
        {
            var result = await _tourPaymentMethodsService.UpdatePaymentMethod(request);
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
    [Route("paymentMethods/{paymentMethodId}")]
    public async Task<IActionResult> GetPaymentMethod([FromRoute] long paymentMethodId)
    {
        GetPaymentMethodRequest request = new(){PaymentMethodId = paymentMethodId};
        try
        {
            var result = await _tourPaymentMethodsService.GetPaymentMethod(request);
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
    [Route("paymentMethods")]
    public async Task<IActionResult> RemovePaymentMethod(RemovePaymentMethodRequest request)
    {
        try
        {
            var result = await _tourPaymentMethodsService.RemovePaymentMethod(request);
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
    
    // [HttpGet]
    // [Route("paymentMethods/page/{pageId}")]
    // public async Task<IActionResult> GetPaymentMethods([FromRoute] int pageId)
    // {
    //     // FIXME: model and method not found
    //     GetPaymentMethodsRequest request = new(){PageId = pageId};
    //     try
    //     {
    //         var result = await _tourPaymentMethodsService.GetPaymentMethods(request);
    //         return Ok(result);
    //     }
    //     catch(Exception ex)
    //     {
    //         if(ex is MyKafkaException)
    //         {
    //             return StatusCode(500, ex.Message);
    //         }
    //         return BadRequest(ex.Message);
    //     }
    // }
}