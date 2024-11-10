using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayService.Controllers.EntertainmentService;

[ApiController]
[Route("api/[controller]")]
public class PhotoController : ControllerBase
{
    public Task<IActionResult> GetPhoto()
    {
        throw new NotImplementedException();
    }

    public IQueryable<IActionResult> GetPhotos()
    {
        throw new NotImplementedException();
    }
}