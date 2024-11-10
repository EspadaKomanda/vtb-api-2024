using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntertaimentService.Kafka.Utils;

namespace EntertaimentService.Kafka
{
    public class PendingMessagesBus
    {
        public string TopicName {get;set;} = "";
        public HashSet<MethodKeyPair> MessageKeys {get;set;} = new HashSet<MethodKeyPair>();
    }
}