using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Exceptions.UserService.Account
{
    [System.Serializable]
    public class CompletePasswordResetException : System.Exception
    {
        public CompletePasswordResetException() { }
        public CompletePasswordResetException(string message) : base(message) { }
        public CompletePasswordResetException(string message, System.Exception inner) : base(message, inner) { }
    }
}