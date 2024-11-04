using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class DeliveryRegionAndAddress
    {
        public string AdministrationZone { get; set; } = null!;
        public PostalAddress PostalAddress { get; set; } = null!;
    }
}