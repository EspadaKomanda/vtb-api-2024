using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace EntertaimentService.KafkaException.ConfigurationException
{
    public class ConfigureConsumersException : MyKafkaException
    {
        public ConfigureConsumersException() {}
        public ConfigureConsumersException(string message) : base(message) {}
        public ConfigureConsumersException(string message, System.Exception inner) : base(message, inner) {}
     }
}