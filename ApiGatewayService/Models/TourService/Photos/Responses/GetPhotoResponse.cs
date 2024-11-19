using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGatewayService.Models.TourService.Models.DTO;

namespace ApiGatewayService.Models.TourService.Models.Photos.Responses
{
    public class GetPhotoResponse
    {
        public PhotoDto Photo { get; set; } = null!;
    }
}