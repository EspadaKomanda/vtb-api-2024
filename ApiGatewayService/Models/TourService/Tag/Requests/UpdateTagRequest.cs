using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Models.TourService.Models.Tag.Requests
{
    public class UpdateTagRequest
    {
        [Required]
        public long TagId { get; set; }
        [Required]
        public string TagName { get; set; } = null!;
    }
}