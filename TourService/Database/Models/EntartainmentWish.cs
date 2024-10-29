using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Database.Models
{
    public class EntartainmentWish
    {
        [Key]
        public long Id {get;set;}
        [Required]
        public long UserId {get;set;}
        [ForeignKey("EntertaimentId")]
        public long EntertaimentId {get;set;}
        [Required]
        public Entertainment Entertaiment {get;set;} = null!;
    }
}