using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses;
public class ErrorResponse
{
    [Required]
    public string code { get; set; } = null!;
    [Required]
    public string id { get; set; } = null!;
    [Required]
    public string message { get; set; } = null!;
    [Required]    
    public List<Error> Errors { get; set; } = null!;
}

