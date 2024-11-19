using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Models.DTO;
using TourService.Models.DTO;

namespace TourService.Models.Photos.Responses
{
    public class GetPhotoResponse
    {
        public PhotoDto Photo { get; set; } = null!;
    }
}