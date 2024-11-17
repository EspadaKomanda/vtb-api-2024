using ApiGatewayService.Services.EntertaimentService.Categories;
using EntertaimentService.Models.Category.Requests;
using Microsoft.AspNetCore.Mvc;
using TourService.KafkaException;

namespace ApiGatewayService.Controllers.EntertainmentService;

[ApiController]
[Route("api/[controller]/ent")]
public class CategoryController(ILogger<CategoryController> logger, IEntertaimentCategoryService entertainmentCategoryService) : ControllerBase
{
    private readonly ILogger<CategoryController> _logger = logger;
    private readonly IEntertaimentCategoryService _tourCategoryService = entertainmentCategoryService;

    [HttpGet]
    [Route("categories/{categoryId}")]
    public async Task<IActionResult> GetCategory([FromRoute] long categoryId)
    {
        GetCategoryRequest request = new(){CategoryId = categoryId};
        try
        {
            var result = await _tourCategoryService.GetCategory(request);
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
    [Route("categories/page/{pageId}")]
    public async Task<IActionResult> GetCategories([FromRoute] int pageId)
    {
        GetCategoriesRequest request = new(){Page = pageId};

        try
        {
            var result = await _tourCategoryService.GetCategories(request);
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