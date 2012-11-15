using System;
using YammerBot.Entity.Yammer;

namespace YammerBot.Core.Yammer.Interface
{
    public interface IYammerMessagePoster
    {
        Message PostMessage(string body);
        Message PostReply(string body, Int64 threadID);
    }
}