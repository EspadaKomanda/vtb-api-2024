using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using TourService.Attributes.Validation;

namespace TourService.Database.Models
{
    [Index(nameof(TourId), nameof(UserId), IsUnique = true)]
    [Index(nameof(EntertainmentId), nameof(UserId), IsUnique = true)]
    public class Review
    {
        [Key]
        public long Id { get; set; }

        public long? TourId { get; set; }
        public Tour? Tour { get; set; }

        public long? EntertainmentId { get; set; }
        public Entertainment? Entertainment { get; set; }

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