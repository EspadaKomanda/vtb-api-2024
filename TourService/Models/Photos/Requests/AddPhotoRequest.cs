using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Models.Photos.Requests
{
    public class AddPhotoRequest
    {
        [Required]
        public long TourId { get; set; }
        [Required]
        public byte[] PhotoBytes { get; set; } = null!;
        [Required]
        public string PhotoName { get; set; } = null!;
    }
}