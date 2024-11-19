using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Models.TourService.Models.Tag.Requests
{
    public class AddTagRequest
    {
        [Required]
        public string TagName { get; set; } = null!;
    }
}