using YammerBot.Entity.Yammer;

namespace YammerBot.Core.Yammer.Interface
{
    public interface IYammerMessageResponseDeserializer
    {
        MessagesFetchResponse DeserializeMessagesResponse(string responseText);
    }
}