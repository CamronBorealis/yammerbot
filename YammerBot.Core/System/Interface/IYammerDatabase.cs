using System.Collections.Generic;
using YammerBot.Entity.Yammer;

namespace YammerBot.Core.System.Interface
{
    public interface IYammerDatabase
    {
        IEnumerable<Message> Messages { get; }
        void AddMessage(Message message);
        void SaveChanges();
    }
}
