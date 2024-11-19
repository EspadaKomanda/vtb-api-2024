using ApiGatewayService.Models.TourService.Models.Benefits;
using ApiGatewayService.Services.TourService.Benefits;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;

namespace ApiGatewayService.Controllers.TourService;

[ApiController]
[Route("api/[controller]")]
public class BenefitController(ITourBenefitsService tourBenefitsService, ILogger<BenefitController> logger) : ControllerBase
{
    private readonly ITourBenefitsService _tourBenefitsService = tourBenefitsService;
    private readonly ILogger<BenefitController> _logger = logger;

    [HttpGet]
    [Route("benefits/{benefitId}")]
    public IActionResult GetBenefit([FromRoute] long benefitId)
    {
        GetBenefitRequest request = new(){BenefitId = benefitId};

        try
        {
            var result = _tourBenefitsService.GetBenefit(request);
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
    [Route("benefits/page/{pageId}")]
    public IActionResult GetBenefits([FromRoute] int pageId)
    {
        GetBenefitsRequest request = new(){Page = pageId};

        try
        {
            var result = _tourBenefitsService.GetBenefits(request);
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