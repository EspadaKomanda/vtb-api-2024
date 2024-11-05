using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGatewayService.Exceptions.UserService.Account
{
    [System.Serializable]
    public class AccountAccessDataException : System.Exception
    {
        public AccountAccessDataException() { }
        public AccountAccessDataException(string message) : base(message) { }
        public AccountAccessDataException(string message, System.Exception inner) : base(message, inner) { }
    }
}