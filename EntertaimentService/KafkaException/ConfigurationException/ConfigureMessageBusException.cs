using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.KafkaException;

namespace EntertaimentService.KafkaException.ConfigurationException
{
   public class ConfigureMessageBusException : MyKafkaException
   {
      public ConfigureMessageBusException() {}
      public ConfigureMessageBusException(string message) : base(message) {}
      public ConfigureMessageBusException(string message, System.Exception inner) : base(message, inner) {}
   }
}