using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Exceptions.UserService.Account
{
    [System.Serializable]
    public class VerifyRegistrationCodeException : System.Exception
    {
        public VerifyRegistrationCodeException() { }
        public VerifyRegistrationCodeException(string message) : base(message) { }
        public VerifyRegistrationCodeException(string message, System.Exception inner) : base(message, inner) { }
    }
}