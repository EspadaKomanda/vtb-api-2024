using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TourService.Attributes.Validation;

namespace TourService.Database.Models
{
    public class Entertainment
    {
        [Key]
        public long Id {get;set;}
        [TourName]
        public string Name {get;set;} = null!;
    }
}