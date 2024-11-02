using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Exceptions.RequestSender;

[System.Serializable]
public class RequestSenderException : System.Exception
{
    public RequestSenderException() { }
    public RequestSenderException(string message) : base(message) { }
    public RequestSenderException(string message, System.Exception inner) : base(message, inner) { }
}
