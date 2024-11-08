using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TourService.Attributes.Validation;

namespace TourService.Models.Tag.Requests
{
    public class AddTagRequest
    {
        [Required]
        [TourName]
        public string TagName { get; set; } = null!;
    }
}