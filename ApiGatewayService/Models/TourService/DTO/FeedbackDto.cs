using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Models.TourService.Models.DTO
{
    public class FeedbackDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public bool IsPositive { get; set; }
    }
}