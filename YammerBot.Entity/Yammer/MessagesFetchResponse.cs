using System.Collections.Generic;
using System.Runtime.Serialization;

namespace YammerBot.Entity.Yammer
{
    [DataContract]
    public class MessagesFetchResponse
    {
        [DataMember(Name = "messages")]
        public virtual List<Message> Messages { get; set; }
    }
}