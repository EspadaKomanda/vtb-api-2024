using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenApiService.Kafka.Utils;

namespace OpenApiService.Kafka;

    public class PendingMessagesBus
    {
        public string TopicName {get;set;} = null!;
        public HashSet<MethodKeyPair> MessageKeys {get;set;}  = null!;
    }
