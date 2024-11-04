using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class DeliveryRegion

{

    public List<string> Comments { get; set; }

    public List<DeliveryRegionAndAddress> DeliveryRegionAndAddress { get; set; }

}
}