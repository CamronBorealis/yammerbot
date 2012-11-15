using System.Collections.Generic;
using YammerBot.Entity.Yammer;

namespace YammerBot.Core.Yammer.Interface
{
    public interface IYammerMessageFetcher
    {
        IEnumerable<Message> GetLatestMessages();
        IEnumerable<Message> GetLatestMessagesAndSaveToDisk(string filePath);
        IEnumerable<Message> GetUnprocessedMessages();
    }
}