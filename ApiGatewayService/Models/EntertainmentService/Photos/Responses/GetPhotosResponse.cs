using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Models.DTO;

namespace EntertaimentService.Models.Photos.Responses
{
    public class GetPhotosResponse
    {
        public int Amount { get; set; }
        public List<PhotoEntertaimentDto>? Photos { get; set; }
    }
}