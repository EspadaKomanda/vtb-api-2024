using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization;
public class Link
{
    [Required]
    [Url]
    public string self { get; set; } = null!;
}
