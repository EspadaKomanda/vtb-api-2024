using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.Photos.Requests
{
    public class GetPhotosEntertaimentRequest
    {
        [Required]
        public long EntertaimentId { get; set; }
    }
}