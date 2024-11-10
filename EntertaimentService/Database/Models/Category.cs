using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Attributes.Validation;

namespace EntertaimentService.Database.Models
{
    public class Category
    {
        [Key]
        public long Id { get; set; }
        [EntertaimentName]
        public string Name {get;set;} = null!;
    }
}