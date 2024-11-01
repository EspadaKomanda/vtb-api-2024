using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UserService.Database.Models;

[Index(nameof(UserId), IsUnique = true)]
public class RegistrationCode
{
    [Key]
    public long Id { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR(6)")]
    public string Code { get; set; } = Guid.NewGuid().ToString();

    public DateTime ExpirationDate { get; set; } = DateTime.Now.AddMinutes(10);

    [Required]
    public User User { get; set; } = null!;
    public long UserId { get; set; }
}