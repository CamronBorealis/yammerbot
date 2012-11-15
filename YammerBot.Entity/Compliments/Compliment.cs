using System.Runtime.Serialization;

namespace YammerBot.Entity.Compliments
{
    [DataContract]
    public class Compliment
    {
        protected bool Equals(Compliment other)
        {
            return string.Equals(Phrase, other.Phrase);
        }

        public override int GetHashCode()
        {
            return (Phrase != null ? Phrase.GetHashCode() : 0);
        }

        [DataMember(Name = "phrase")]
        public string Phrase { get; set; }

        public override bool Equals(object obj)
        {
            return Equals((Compliment) obj);
        }
    }
}