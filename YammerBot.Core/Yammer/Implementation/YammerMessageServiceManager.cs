using System;
using System.Collections;
using System.Collections.Generic;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Storage.Basic;
using YammerBot.Core.System.Interface;
using YammerBot.Core.Yammer.Interface;

namespace YammerBot.Core.Yammer.Implementation
{
    public class YammerMessageServiceManager:IYammerMessageServiceManager
    {
        private readonly IYammerService _service;

        public YammerMessageServiceManager(IYammerService service)
        {
            _service = service;
        }

        public string GetLatestMessagesTextFromServer()
        {
            var responseText = _service.GetMessages(null);
            return responseText;
        }

        public string GetLatestMessagesTextFromServerAfterMessageID(Int64 messageID)
        {
            var parameters = new Dictionary<string, string>
                                 {
                                     {"older_than", messageID.ToString()}
                                 };
            return _service.GetMessages(parameters);
        }

        public string PostNewMessage(string message)
        {
            var parameters = new Dictionary<string, string> { { "body", message } };
            return _service.PostMessage(parameters);
        }

        public string PostMessageReply(string message, Int64 threadId)
        {
            var parameters = new Dictionary<string, string> { { "body", message }, { "replied_to_id", threadId.ToString() } };
            return _service.PostMessage(parameters);
        }
    }
}
