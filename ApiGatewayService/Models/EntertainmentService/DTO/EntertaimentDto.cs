using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3.Model;
using EntertaimentService.Models.Category;
using EntertaimentService.Models.PaymentMethod;

namespace EntertaimentService.Models.DTO
{
    public class EntertaimentDto
    {        
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price {get; set; }
        public string Address  { get; set; } = null!;
        public string Coordinates { get; set; } = null!;
        public double? SettlementDistance {get; set; }
        public string? Comment { get; set; }
        public bool IsActive { get; set; }
        public List<PhotoEntertaimentDto>? Photos { get; set; }
        public List<ReviewEntertaimentDto>? Reviews { get; set; }
        public List<CategoryEntertaimentDto>? Categories { get; set; }
        public List<TagEntertaimentDto>? Tags { get; set; }
        public List<PaymentMethodEntertaimentDto>? PaymentMethods { get; set; }
    }
}