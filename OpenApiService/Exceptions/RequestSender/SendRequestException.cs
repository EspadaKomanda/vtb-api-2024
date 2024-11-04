using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Exceptions.RequestSender;

[System.Serializable]
public class SendRequestException : RequestSenderException
{
    public SendRequestException() { }
    public SendRequestException(string message) : base(message) { }
    public SendRequestException(string message, System.Exception inner) : base(message, inner) { }

}
