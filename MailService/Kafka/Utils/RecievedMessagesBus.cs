using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace TourService.Kafka
{
    public class RecievedMessagesBus
    {
        public string TopicName { get; set; } = "";
        public HashSet<Message<string,string>> Messages { get; set;} = new HashSet<Message<string,string>>();
    }
}