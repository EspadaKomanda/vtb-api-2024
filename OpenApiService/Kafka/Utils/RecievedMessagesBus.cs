using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace OpenApiService.Kafka;

    public class RecievedMessagesBus
    {
        public string TopicName { get; set; } = null!;
        public HashSet<Message<string,string>> Messages { get; set;} = null!;
    }
