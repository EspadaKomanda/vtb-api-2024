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
        //TODO: Add validation attributes
        [Key]
        public long Id { get; set; }
        public long UserId { get; set; }
        [ForeignKey("TourId")]
        public long TourId { get; set; }
        public Tour Tour { get; set; }
    }
}