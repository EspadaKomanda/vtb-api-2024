using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class PostalAddress
    {
        public string AddressType { get; set; } = null!;
        public List<string> AddressLine { get; set; } = null!;
        public string StreetName { get; set; } = null!;
        public string BuildingNumber { get; set; } = null!;
        public string PostCode { get; set; } = null!;
        public string TownName { get; set; } = null!;
        public string CountrySubDivision { get; set; } = null!;
        public string Country { get; set; } = null!;

    }

}