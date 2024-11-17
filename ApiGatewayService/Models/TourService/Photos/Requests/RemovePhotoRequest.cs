using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Models.TourService.Models.Photos.Requests
{
    public class RemovePhotoRequest
    {
        [Required]
        public long PhotoId { get; set; }
    }
}