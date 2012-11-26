using System;
using YammerBot.Core.Yammer.Interface;
using YammerBot.Entity.Yammer;

namespace YammerBot.Core.Yammer.Implementation
{
    public class YammerMessagePoster:IYammerMessagePoster
    {
        private IYammerServiceManager _serviceManager;
        private IYammerMessageResponseDeserializer _deserializer;

        public YammerMessagePoster(IYammerServiceManager serviceManager, IYammerMessageResponseDeserializer deserializer)
        {
            _serviceManager = serviceManager;
            _deserializer = deserializer;
        }

        public Message PostMessage(string body)
        {
            var rawResponse = _serviceManager.PostNewMessage(body);
            var response = _deserializer.DeserializeMessagesResponse(rawResponse);
            return response.Messages[0];
        }

        public Message PostReply(string body, Int64 threadID)
        {
            var rawResponse = _serviceManager.PostMessageReply(body, threadID);
            var response = _deserializer.DeserializeMessagesResponse(rawResponse);
            return response.Messages[0];
        }
    }
}
