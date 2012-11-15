namespace YammerBot.Core.OAuth.Interface
{
    public interface IOauthValueProvider
    {
        string ConsumerKey { get; }
        string ConsumerSecret { get; }
        string Token { get; }
        string TokenSecret { get; }
    }
}
