using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.Models.User.Requests
{
    public class GetUserEntertainmentName
    {
        [Required]
        public long UserId { get; set; }
    }
}