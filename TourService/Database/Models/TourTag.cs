using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Database.Models
{
    public class TourTag
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("TourId")]
        public long TourId { get; set; }
        [Required]
        public Tour Tour { get; set; } = null!;
        [ForeignKey("TagId")]
        public long TagId { get; set; }
        [Required]
        public Tag Tag{ get; set; } = null!;
    }
}