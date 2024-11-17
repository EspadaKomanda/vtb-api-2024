using ApiGatewayService.Services.TourService.Categories;
using Microsoft.AspNetCore.Mvc;
using ApiGatewayService.Models.TourService.Models.Category.Requests;
using TourService.KafkaException;

namespace ApiGatewayService.Controllers.TourService;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ILogger<CategoryController> logger, ITourCategoryService tourCategoryService) : ControllerBase
{
    private readonly ILogger<CategoryController> _logger = logger;
    private readonly ITourCategoryService _tourCategoryService = tourCategoryService;

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