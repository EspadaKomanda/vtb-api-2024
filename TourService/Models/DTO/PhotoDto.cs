using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Models.DTO
{
    public class PhotoDto
    {
        public long PhotoId { get; set; }
        public string PhotoName {get;set;} = null!;
        public long TourId { get; set; }
        public string FileLink { get; set; } = null!;
    }
}