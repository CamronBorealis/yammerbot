namespace YammerBot.Core.Yammer.Interface
{
    public interface IYammerTaskRunner
    {
        void GetAccessToken();
        void ExecuteReplies();
    }
}