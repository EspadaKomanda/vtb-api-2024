using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Exceptions.UserService.Account
{
    [System.Serializable]
    public class ResendPasswordResetCodeException : System.Exception
    {
        public ResendPasswordResetCodeException() { }
        public ResendPasswordResetCodeException(string message) : base(message) { }
        public ResendPasswordResetCodeException(string message, System.Exception inner) : base(message, inner) { }
    }
}