using System.Collections;

namespace YammerBot.Core.System.Interface
{
    public interface IYammerService
    {
        string GetMessages(IDictionary parameters);
        string PostMessage(IDictionary parameters);
    }
}
