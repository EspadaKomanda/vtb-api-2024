using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses;
public class Error
{
    [Required]
    public string errorCode { get; set; } = null!;
    [Required]
    public string message { get; set; } = null!;
    [Required]
    public string path { get; set; } = null!;
    [Required]
    public string url { get; set; } = null!;
}
