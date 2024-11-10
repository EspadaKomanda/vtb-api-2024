
using UserService.Attributes.Validation;

namespace UserService.Models.Profile.Requests;

public class UpdateProfileRequest
{
    [ValidName]
    public string? Name { get; set; }
    [ValidName]
    public string? Surname { get; set; }
    [ValidName]
    public string? Patronymic { get; set; }
    [ValidAge]
    public DateTime? Birthday { get; set; }
    [ValidAvatar]
    public string? Avatar { get; set; }
}