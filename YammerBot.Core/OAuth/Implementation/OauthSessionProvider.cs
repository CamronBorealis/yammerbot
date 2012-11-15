using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Storage.Basic;
using YammerBot.Core.OAuth.Interface;

namespace YammerBot.Core.OAuth.Implementation
{
    public class OauthSessionProvider : IOauthSessionProvider
    {
        private readonly IOauthValueProvider _valueProvider;
        private const string RequestUrl = "https://www.yammer.com/oauth/request_token";
        private const string UserAuthorizeUrl = "https://www.yammer.com/oauth/authorize";
        private const string AccessUrl = "https://www.yammer.com/oauth/access_token";

        private readonly OAuthSession _session;

        public OauthSessionProvider(IOauthValueProvider valueProvider)
        {
            _valueProvider = valueProvider;
            var context = new OAuthConsumerContext
            {
                ConsumerKey = _valueProvider.ConsumerKey,
                ConsumerSecret = _valueProvider.ConsumerSecret,
                SignatureMethod = SignatureMethod.PlainText,
                UseHeaderForOAuthParameters = true
            };
            _session = new OAuthSession(context, RequestUrl, UserAuthorizeUrl, AccessUrl);
            var accessToken = new AccessToken
            {
                Token = _valueProvider.Token,
                TokenSecret = _valueProvider.TokenSecret
            };
            _session.AccessToken = accessToken;
        }

        public OAuthSession GetOAuthSession()
        {
            return _session;
        }
    }
}
