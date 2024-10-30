using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Database.Models
{
    public class TourWish
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [ForeignKey("TourId")]
        public long TourId { get; set; }
        [Required]
        public Tour Tour { get; set; } = null!;
    }
}