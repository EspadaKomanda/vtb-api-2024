using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGatewayService.Models.TourService.Models.DTO;

namespace ApiGatewayService.Models.TourService.Models.Tag.Responses
{
    public class GetTagResponse
    {
        public TagDto Tag { get; set; } = null!;
    }
}