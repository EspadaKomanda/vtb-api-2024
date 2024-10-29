using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Database.Models
{
    public class Review
    {
        //TODO: Add validation attributes
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        public int Rating {get;set;}
        public string Text {get; set;}
        public bool IsAnonymous {get; set;}
        public bool Removed {get; set;}
    }
}