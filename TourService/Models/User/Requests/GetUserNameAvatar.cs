using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Models.User.Requests
{
    public class GetUserNameAvatar
    {
        [Required]
        public long UserId { get; set; }
    }
}