using System.Collections.Generic;
using System.Linq;
using YammerBot.Core.System.Interface;
using YammerBot.Core.Yammer.Interface;
using YammerBot.Entity.Yammer;

namespace YammerBot.Core.Yammer.Implementation
{
    public class YammerMessageFetcher:IYammerMessageFetcher
    {
        private readonly IYammerMessageServiceManager _serviceManager;
        private readonly IYammerMessageResponseDeserializer _deserializer;
        private readonly IYammerMessageDatabaseManager _databaseManager;
        private readonly IFileDataProvider _fileDataProvider;

        public YammerMessageFetcher(IYammerMessageServiceManager serviceManager, IYammerMessageResponseDeserializer deserializer,
            IYammerMessageDatabaseManager databaseManager, IFileDataProvider fileDataProvider)
        {
            _serviceManager = serviceManager;
            _deserializer = deserializer;
            _databaseManager = databaseManager;
            _fileDataProvider = fileDataProvider;
        }

        public IEnumerable<Message> GetLatestMessages()
        {
            var responseText = _serviceManager.GetLatestMessagesTextFromServer();
            var responseObject = _deserializer.DeserializeMessagesResponse(responseText);
            return responseObject.Messages;
        }

        public IEnumerable<Message> GetLatestMessagesAndSaveToDisk(string filePath)
        {
            var responseText = _serviceManager.GetLatestMessagesTextFromServer();
            _fileDataProvider.WriteAllText(filePath, responseText);
            var responseObject = _deserializer.DeserializeMessagesResponse(responseText);
            return responseObject.Messages;
        }

        public IEnumerable<Message> GetUnprocessedMessages()
        {
            var messageList = new List<Message>();
            var responseText = _serviceManager.GetLatestMessagesTextFromServer();
            var responseObject = _deserializer.DeserializeMessagesResponse(responseText);
            bool needToFetchMore = true;
            while(needToFetchMore)
            {
                foreach (var message in responseObject.Messages)
                {
                    if (_databaseManager.IsMessageInDatabase(message.ID))
                    {
                        needToFetchMore = false;
                        break;
                    }
                    else
                        messageList.Add(message);
                }
                if (needToFetchMore)
                {
                    responseText = _serviceManager.GetLatestMessagesTextFromServerAfterMessageID(messageList.Min(m=>m.ID));
                    responseObject = _deserializer.DeserializeMessagesResponse(responseText);
                }
            }
            _databaseManager.SaveMessages(messageList);
            _databaseManager.SaveChanges();
            return messageList;
        }
    }
}
