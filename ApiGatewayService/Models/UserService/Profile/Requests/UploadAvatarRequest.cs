using System.ComponentModel.DataAnnotations;

namespace ApiGatewayService.Models.UserService.Profile.Requests;

public class UploadAvatarRequest
{
    [Required]
    public Byte[] Avatar { get; set; } = null!;
}