using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Database.Models
{
    public class EntertaimentCategory
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("EntertaimentId")]
        public long EntertaimentId { get; set; }
        [Required]
        public Entertaiment Tour { get; set; } = null!;
        [ForeignKey("CategoryId")]
        public long CategoryId { get; set; }
        [Required]
        public Category Category{ get; set; } = null!;
    }
}