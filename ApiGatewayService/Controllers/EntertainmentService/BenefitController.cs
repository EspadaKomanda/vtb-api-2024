using ApiGatewayService.Services.EntertaimentService.Benefits;
using EntertaimentService.Models.Benefits;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;

namespace ApiGatewayService.Controllers.EntertainmentService;

[ApiController]
[Route("api/[controller]/ent")]
public class BenefitController(IEntertaimentBenefitService entertainmentBenefitsService, ILogger<BenefitController> logger) : ControllerBase
{
    private readonly IEntertaimentBenefitService _entertainmentBenefitsService = entertainmentBenefitsService;
    private readonly ILogger<BenefitController> _logger = logger;

    [HttpGet]
    [Route("benefits/{benefitId}")]
    public IActionResult GetBenefit([FromRoute] long benefitId)
    {
        GetBenefitRequest request = new(){BenefitId = benefitId};

        try
        {
            var result = _entertainmentBenefitsService.GetBenefit(request);
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
            var result = _entertainmentBenefitsService.GetBenefits(request);
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