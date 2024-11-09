using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Attributes.Validation;

namespace EntertaimentService.Database.Models
{
    public class Photo
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("EntertaimentId")]
        public long EntertaimentId { get; set; }
        public Entertaiment Entertaiment { get; set; } = null!;
        [FileLink]
        public string FileLink {get;set;} = null!;
        public string Title { get; set; } = null!;
    }
}