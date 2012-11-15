using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YammerBot.Entity.Yammer;

namespace YammerBot.Core.Yammer.Interface
{
    public interface IYammerCommandFetcher
    {
        IDictionary<string, Func<Message, Message>> GetCommands();
    }
}
