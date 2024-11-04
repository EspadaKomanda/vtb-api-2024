using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Models.Responses.CreditOrganization
{
    public class Links
    {
        [Url]
        public string Self { get; set; }
        [Url]
        public string First { get; set; }
        [Url]
        public string Prev { get; set; }
        [Url]
        public string Next { get; set; }
        [Url]
        public string Last { get; set; }
    }
}
