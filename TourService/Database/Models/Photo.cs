using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TourService.Attributes.Validation;

namespace TourService.Database.Models
{
    public class Photo
    {
        //TODO: Add validation attributes
        [Key]
        public long Id { get; set; }
        [FileLink]
        public string FileLink {get;set;} = null!;
    }
}