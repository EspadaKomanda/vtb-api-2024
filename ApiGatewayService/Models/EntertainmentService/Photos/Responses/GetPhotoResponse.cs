using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Models.DTO;

namespace EntertaimentService.Models.Photos.Responses
{
    public class GetPhotoResponse
    {
        public PhotoEntertaimentDto Photo { get; set; } = null!;
    }
}