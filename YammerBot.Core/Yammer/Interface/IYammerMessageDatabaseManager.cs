using System;
using System.Collections.Generic;
using YammerBot.Entity.Yammer;

namespace YammerBot.Core.Yammer.Interface
{
    public interface IYammerMessageDatabaseManager
    {
        void SaveMessages(IEnumerable<Message> messages);
        bool IsMessageInDatabase(Int64 messageID);
        void SaveChanges();
    }
}