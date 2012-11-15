using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace YammerBot.Entity.Yammer
{
    [DataContract]
    public class Message
    {
        [DataMember(Name = "id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }
        [DataMember(Name = "body")]
        public MessageBody Body { get; set; }
    }
}
