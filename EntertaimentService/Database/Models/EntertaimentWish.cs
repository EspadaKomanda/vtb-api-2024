using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Database.Models
{
    public class EntertaimentWish
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [ForeignKey("EntertaimentId")]
        public long EntertaimentId { get; set; }
        [Required]
        public Entertaiment Entertaiment { get; set; } = null!;
    }
}