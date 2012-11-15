using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YammerBot.Core.Compliment.Interface;

namespace YammerBot.Core.Compliment.Implementation
{
    public class ComplimentTextDeserializer : IComplimentTextDeserializer
    {
        public List<Entity.Compliments.Compliment> GetComplimentsFromComplimentText(string text)
        {
            var serializer = new JsonSerializer();
            var stringReader = new StringReader(text);
            var jsonTextReader = new JsonTextReader(stringReader);
            return (List<Entity.Compliments.Compliment>)serializer.Deserialize(jsonTextReader, typeof(List<Entity.Compliments.Compliment>));
        }
    }
}
