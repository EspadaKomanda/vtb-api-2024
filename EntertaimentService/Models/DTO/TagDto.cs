using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Models.DTO
{
    public class TagDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
    }
}