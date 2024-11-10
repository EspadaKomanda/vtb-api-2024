using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Models.User.Responses
{
    public class GetUsernameAvatarResponse
    {
        public string Username { get; set; } = null!;
        public string Avatar { get; set; } = null!;
    }
}