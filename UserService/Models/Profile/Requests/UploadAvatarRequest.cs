using System.ComponentModel.DataAnnotations;

namespace UserService.Models.Profile.Requests;

public class UploadAvatarRequest
{
    public long UserId { get; set; }
    [Required]
    public Byte[] Avatar { get; set; } = null!;
}