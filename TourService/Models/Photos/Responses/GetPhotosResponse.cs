using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourService.Models.DTO;

namespace TourService.Models.Photos.Responses
{
    public class GetPhotosResponse
    {
        public int Amount { get; set; }
        public List<PhotoDto>? Photos { get; set; }
    }
}