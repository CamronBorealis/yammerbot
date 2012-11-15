using System.Collections.Generic;
using YammerBot.Core.System.Interface;
using YammerBot.Core.Yammer.Interface;
using YammerBot.Entity.Yammer;
using YammerBot.SQL;

namespace YammerBot.Core.System.Implementation
{
    public class YammerDatabase:IYammerDatabase
    {
        private readonly IEnvironmentInfoProvider _environmentInfoProvider;
        private YammerBotContext _context;

        public YammerDatabase(IEnvironmentInfoProvider environmentInfoProvider)
        {
            _environmentInfoProvider = environmentInfoProvider;
            _context = new YammerBotContext();
            _context.Database.Connection.ConnectionString = @"Server=(localdb)\v11.0;Integrated Security=true;AttachDbFileName=" + _environmentInfoProvider.DataDirectory + @"\Data.mdf;Database=YammerBotData";
        }

        public IEnumerable<Message> Messages
        {
            get { return _context.Messages; }
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
