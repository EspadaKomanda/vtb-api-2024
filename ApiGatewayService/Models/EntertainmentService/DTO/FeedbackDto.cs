using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.DTO
{
    public class FeedbackEntertaimentDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public bool IsPositive { get; set; }
    }
}