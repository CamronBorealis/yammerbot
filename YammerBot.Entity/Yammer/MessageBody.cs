using System.Collections.Generic;
using System.Runtime.Serialization;

namespace YammerBot.Entity.Yammer
{
    [DataContract]
    public class MessageBody
    {
        [DataMember(Name = "plain")]
        public string Plain { get; set; }
    }
}