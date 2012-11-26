using System.Collections;

namespace YammerBot.Core.Yammer.Interface
{
    public interface IYammerService
    {
        string GetMessages(IDictionary parameters);
        string PostMessage(IDictionary parameters);
    }
}
