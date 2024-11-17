using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.Photos.Requests
{
    public class AddPhotoEntertaimentRequest
    {
        [Required]
        public long EntertaimentId { get; set; }
        [Required]
        public byte[] PhotoBytes { get; set; } = null!;
        [Required]
        public string PhotoName { get; set; } = null!;
    }
}