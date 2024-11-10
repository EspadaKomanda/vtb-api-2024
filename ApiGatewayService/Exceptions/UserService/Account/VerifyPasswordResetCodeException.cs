using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Exceptions.UserService.Account
{
    [System.Serializable]
    public class VerifyPasswordResetCodeException : System.Exception
    {
        public VerifyPasswordResetCodeException() { }
        public VerifyPasswordResetCodeException(string message) : base(message) { }
        public VerifyPasswordResetCodeException(string message, System.Exception inner) : base(message, inner) { }
    }
}