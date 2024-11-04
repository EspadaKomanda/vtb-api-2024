using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class ProductDetails
    {
        public bool Active { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ActiveFrom { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ActiveTo { get; set; }
        public int FeeFreeLength { get; set; }
        public string FeeFreeLengthPeriod { get; set; } = null!;
        public bool ProductInsurance { get; set; }
        public List<DeliveryRegion> DeliveryRegions { get; set; }
        public List<FeatureAndBenefit> FeatureAndBenefit { get; set; }
        public List<Eligibility> Eligibility { get; set; }
        public Insurance Insurance { get; set; }
        public List<string> Comments { get; set; } = new List<string>();
    }
}