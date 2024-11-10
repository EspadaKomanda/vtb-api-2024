using System.ComponentModel.DataAnnotations;
using TourService.Attributes.Validation;

namespace TourService.Database.Models
{
    public class Benefit
    {
        [Key]
        public long Id { get; set; }
        [TourName]
        public string Name { get; set; } = null!;
    }
}