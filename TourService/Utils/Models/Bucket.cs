using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Utils.Models
{
    public class Bucket
    {
        public string BucketName { get; set; } = null!;
        public Guid BucketId { get; set;}
    }
}