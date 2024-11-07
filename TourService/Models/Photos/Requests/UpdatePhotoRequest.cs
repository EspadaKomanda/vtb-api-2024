using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Models.Photos.Requests
{
    public class UpdatePhotoRequest
    {
        [Required]
        public long PhotoId { get; set; } 
        [Required]
        public byte[] PhotoBytes {get;set;} = null!;
    }
}