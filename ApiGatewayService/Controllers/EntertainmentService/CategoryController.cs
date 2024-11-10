using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayService.Controllers.EntertainmentService;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    public Task<IActionResult> GetCategory()
    {
        throw new NotImplementedException();
    }

    public IQueryable<IActionResult> GetCategories()
    {
        throw new NotImplementedException();
    }
}