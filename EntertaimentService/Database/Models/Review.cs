using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using EntertaimentService.Attributes.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntertaimentService.Database.Models
{
    [Index(nameof(EntertaimentId), nameof(UserId), IsUnique = true)]
    public class Review
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [ForeignKey("EntertaimentId")]
        public long EntertaimentId { get; set; }
        public Entertaiment Entertaiment { get; set; } = null!;
        
        public long? UserId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Rating(1,10)]
        public int Rating {get;set;}

        [EntertaimentText]
        public string Text {get; set;} = null!;

        [Required]
        public bool IsAnonymous {get; set;}

        [Required]
        public bool Removed {get; set;}
    }
}