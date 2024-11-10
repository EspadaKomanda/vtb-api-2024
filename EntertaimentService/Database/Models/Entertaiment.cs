using System.ComponentModel.DataAnnotations;
using EntertaimentService.Attributes.Validation;

namespace EntertaimentService.Database.Models
{
    public class Entertaiment
    {
        [Key]
        public long Id { get; set; }
        [EntertaimentName]
        public string Name { get; set; } = null!;
        [EntertaimentDescription]
        public string Description { get; set; } = null!;
        [EntertaimentName]
        public double Price {get; set; }
        [EntertaimentAddress]
        public string Address  { get; set; } = null!;
        [Coordinates]
        public string Coordinates { get; set; } = null!;
        [Required]
        public double SettlementDistance {get; set; }
        [Required]
        public bool IsActive { get; set; }
        public double Rating { get; set; }
        [EntertaimentText]
        public string? Comment { get; set;} = null!;
        public ICollection<Photo>? Photos { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}