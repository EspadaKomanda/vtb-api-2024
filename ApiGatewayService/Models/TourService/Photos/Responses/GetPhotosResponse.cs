using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGatewayService.Models.TourService.Models.DTO;

namespace ApiGatewayService.Models.TourService.Models.Photos.Responses
{
    public class GetPhotosResponse
    {
        public int Amount { get; set; }
        public List<PhotoDto>? Photos { get; set; }
    }
}