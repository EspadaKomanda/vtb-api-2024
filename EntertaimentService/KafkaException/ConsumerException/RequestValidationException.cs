using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertaimentService.KafkaException.ConsumerException
{
    public class RequestValidationException : ConsumerException
    {
        public RequestValidationException()
        {
        }

        public RequestValidationException(string message)
            : base(message)
        {
        }

        public RequestValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}