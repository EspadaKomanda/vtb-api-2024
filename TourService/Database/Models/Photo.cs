using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TourService.Attributes.Validation;

namespace TourService.Database.Models
{
    public class Photo
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("TourId")]
        public long TourId { get; set; }
        public Tour Tour { get; set; } = null!;
        [FileLink]
        public string FileLink {get;set;} = null!;
    }
}