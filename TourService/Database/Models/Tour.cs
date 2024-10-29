using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Database.Models
{
    public class Tour
    {
        //TODO: Add validation attributes
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price {get; set; }
        public string Address  { get; set; }
        public string Coordinates { get; set; }
        public double SettlementDistance {get; set; }
        public bool IsActive { get; set; }
        public string Comment { get; set;}
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}