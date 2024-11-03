using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class GetProductsResponse
    {
        [Required]
        public ProductData Data { get; set; } = null!;
        [Required]
        public List<Link> Links{ get; set; } = null!;
        [Required]
        public Meta Meta{ get; set; } = null!;
    }
}