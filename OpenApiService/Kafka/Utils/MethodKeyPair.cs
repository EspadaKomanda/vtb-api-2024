using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiService.Kafka.Utils;

public class MethodKeyPair
{
    public string MessageKey { get; set; } = null!;
    public string MessageMethod {get;set;} = null!;
}
