using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Exceptions.UserService.Account
{
    [System.Serializable]
    public class BeginPasswordResetException : System.Exception
    {
        public BeginPasswordResetException() { }
        public BeginPasswordResetException(string message) : base(message) { }
        public BeginPasswordResetException(string message, System.Exception inner) : base(message, inner) { }
    }
}