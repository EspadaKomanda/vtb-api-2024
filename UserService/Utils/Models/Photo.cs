using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Database.Models
{
    public class Photo
    {
        public string FileLink {get;set;} = null!;
        public string Title { get; set; } = null!;
    }
}