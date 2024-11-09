using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.DTO
{
    public class PhotoDto
    {
        public long PhotoId { get; set; }
        public string PhotoName {get;set;} = null!;
        public long EntertaimentId { get; set; }
        public string FileLink { get; set; } = null!;
    }
}