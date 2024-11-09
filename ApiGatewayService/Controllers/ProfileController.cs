using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGatewayService.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace ApiGatewayService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController(ILogger logger) : ControllerBase
    {
    }
}
