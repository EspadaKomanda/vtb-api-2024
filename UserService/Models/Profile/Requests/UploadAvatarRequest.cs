using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Profile.Requests;

public class UploadAvatarRequest
{
    [Required]
    public Byte[] Avatar { get; set; } = null!;
}