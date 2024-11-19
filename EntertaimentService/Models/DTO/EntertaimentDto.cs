using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3.Model;
using EntertaimentService.Models.Category;
using EntertaimentService.Models.PaymentMethod;
using TourService.Models.Category;
using TourService.Models.PaymentMethod;

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
        public List<PhotoDto>? Photos { get; set; }
        public List<ReviewDto>? Reviews { get; set; }
        public List<CategoryDto>? Categories { get; set; }
        public List<TagDto>? Tags { get; set; }
        public List<PaymentMethodDto>? PaymentMethods { get; set; }
    }
}