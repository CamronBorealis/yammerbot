using System;
using System.Collections.Generic;
using System.Linq;
using YammerBot.Core.System.Interface;
using YammerBot.Core.Yammer.Interface;
using YammerBot.Entity.Yammer;

namespace YammerBot.Core.Yammer.Implementation
{
    public class YammerMessageDatabaseManager : IYammerMessageDatabaseManager
    {
        private readonly IYammerDatabase _database;

        public YammerMessageDatabaseManager(IYammerDatabase database)
        {
            _database = database;
        }

        public void SaveMessages(IEnumerable<Message> messages)
        {
            foreach (var message in messages)
                _database.AddMessage(message);
        }

        public bool IsMessageInDatabase(Int64 messageID)
        {
            return _database.Messages.Any(a => a.ID == messageID);
        }

        public void SaveChanges()
        {
            _database.SaveChanges();
        }
    }
}
