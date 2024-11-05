using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Exceptions.UserService.Account
{
    [System.Serializable]
    public class BeginRegistrationException : System.Exception
    {
        public BeginRegistrationException() { }
        public BeginRegistrationException(string message) : base(message) { }
        public BeginRegistrationException(string message, System.Exception inner) : base(message, inner) { }
        protected BeginRegistrationException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}