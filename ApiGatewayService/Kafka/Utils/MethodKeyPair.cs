using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TourService.Kafka.Utils
{
    public class MethodKeyPair
    {
        public string MessageKey { get; set; } = "";
        public string MessageMethod {get;set;} = "";
    }
}