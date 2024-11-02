using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Exceptions.RequestSender;
[System.Serializable]
public class BuildUriException : RequestSenderException
{
    public BuildUriException() { }
    public BuildUriException(string message) : base(message) { }
    public BuildUriException(string message, System.Exception inner) : base(message, inner) { }

}
