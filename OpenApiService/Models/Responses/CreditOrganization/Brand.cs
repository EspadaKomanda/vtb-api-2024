using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization;

public class Brand
{
    [Required]
    [StringLength(100)]
    public string brandName { get; set; } = null!;

    [Required]
    [Url]
    public string applicationUri { get; set; } = null!;
}