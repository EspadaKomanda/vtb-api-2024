using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGatewayService.Models.TourService.Models.DTO;

namespace TourService.Models.Tag.Responses
{
    public class GetTagsResponse
    {
        public List<TagDto>? Tags { get; set; }
    }
}