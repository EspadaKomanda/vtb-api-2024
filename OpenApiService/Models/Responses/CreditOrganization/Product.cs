using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization;
public class Product
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [StringLength(100)]
    public string ProductName { get; set; } = null!;

    [Required]
    public string ProductType { get; set; } = null!;

    [Required]
    public string ProductVersion { get; set; } = null!;

    [Required]
    public List<Brand> Brand { get; set; } = null!;
}
