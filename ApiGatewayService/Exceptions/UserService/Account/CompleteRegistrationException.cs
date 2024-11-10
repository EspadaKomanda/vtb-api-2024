using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Exceptions.UserService.Account
{
    [System.Serializable]
    public class CompleteRegistrationException : System.Exception
    {
        public CompleteRegistrationException() { }
        public CompleteRegistrationException(string message) : base(message) { }
        public CompleteRegistrationException(string message, System.Exception inner) : base(message, inner) { }
    }
}