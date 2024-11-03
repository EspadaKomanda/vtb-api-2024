using System.ComponentModel.DataAnnotations;
using TourService.Attributes.Validation;

namespace TourService.Database.Models
{
    public class Review
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Rating(1,10)]
        public int Rating {get;set;}
        [Text]
        public string Text {get; set;} = null!;
        [Required]
        public bool IsAnonymous {get; set;}
        [Required]
        public bool Removed {get; set;}
    }
}