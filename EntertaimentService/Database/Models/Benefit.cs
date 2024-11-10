using System.ComponentModel.DataAnnotations;
using EntertaimentService.Attributes.Validation;

namespace EntertaimentService.Database.Models
{
    public class Benefit
    {
        [Key]
        public long Id { get; set; }
        [EntertaimentName]
        public string Name { get; set; } = null!;
    }
}