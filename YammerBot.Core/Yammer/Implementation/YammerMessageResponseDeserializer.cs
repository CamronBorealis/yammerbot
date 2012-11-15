using System.IO;
using System.Text;
using Newtonsoft.Json;
using YammerBot.Core.Yammer.Interface;
using YammerBot.Entity.Yammer;

namespace YammerBot.Core.Yammer.Implementation
{
    public class YammerMessageResponseDeserializer : IYammerMessageResponseDeserializer
    {
        public MessagesFetchResponse DeserializeMessagesResponse(string responseText)
        {
            using (var stream = new MemoryStream())
            {
                using(var writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.Write(responseText);
                    writer.Flush();
                }
                var jsonSerializer = new JsonSerializer();
                using (TextReader textReader = new StringReader(responseText))
                {
                    JsonReader reader = new JsonTextReader(textReader);
                    return (MessagesFetchResponse)jsonSerializer.Deserialize(reader, typeof(MessagesFetchResponse));
                }
            }
        }
    }
}
