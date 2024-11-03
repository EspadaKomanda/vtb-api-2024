using System.ComponentModel.DataAnnotations;
using TourService.Attributes.Validation;

namespace TourService.Database.Models
{
    public class Tour
    {
        [Key]
        public long Id { get; set; }
        [TourName]
        public string Name { get; set; } = null!;
        [TourDescription]
        public string Description { get; set; } = null!;
        [TourName]
        public double Price {get; set; }
        [TourAddress]
        public string Address  { get; set; } = null!;
        [Coordinates]
        public string Coordinates { get; set; } = null!;
        [Required]
        public double SettlementDistance {get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Text]
        public string? Comment { get; set;} = null!;
        public ICollection<Photo>? Photos { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}