using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization;
public class  Product
{
    [Required]
    public Guid productId { get; set; }
    [Required]
    [StringLength(100)]
    public string productName { get; set; } = null!;
    [Required]
    public string productType { get; set; } = null!;
    [Required]
    public string productVersion { get; set; } = null!;
    [Required]
    public Brand Brand { get; set; } = null!;
    public ProductDetails? ProductDetails { get; set; } = null!;
    public DebitInterest? DebitInterest { get; set; }
    public CreditInterest? CreditInterest { get; set; }   
    public Overdraft? Overdraft { get; set; }
    public List<Repayment>? Repayment { get; set; }
    public List<OtherFeesCharges>? OtherFeesCharges { get; set; }
    public SupplementaryData? SupplementaryData { get; set; }
}
