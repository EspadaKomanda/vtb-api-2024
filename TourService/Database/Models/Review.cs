using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TourService.Attributes.Validation;

namespace TourService.Database.Models
{
    public class Review
    {
        //TODO: Add validation attributes
        [Key]
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Rating(1,10)]
        public int Rating {get;set;}
        public string Text {get; set;}
        public bool IsAnonymous {get; set;}
        public bool Removed {get; set;}
    }
}