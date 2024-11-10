using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Exceptions.UserService.Account
{
    [System.Serializable]
    public class ResendRegistrationCodeException : System.Exception
    {
        public ResendRegistrationCodeException() { }
        public ResendRegistrationCodeException(string message) : base(message) { }
        public ResendRegistrationCodeException(string message, System.Exception inner) : base(message, inner) { }
    }
}