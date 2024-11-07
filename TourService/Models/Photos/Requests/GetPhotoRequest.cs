using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NJsonSchema.Annotations;

namespace TourService.Models.Photos.Requests
{
    public class GetPhotoRequest
    {
        [Required]
        public long PhotoId { get; set; }
    }
}