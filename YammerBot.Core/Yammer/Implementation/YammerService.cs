using System.Collections;
using DevDefined.OAuth.Consumer;
using YammerBot.Core.OAuth.Interface;
using YammerBot.Core.Yammer.Interface;

namespace YammerBot.Core.Yammer.Implementation
{
    public class YammerService:IYammerService
    {
        private readonly IOauthSessionProvider _sessionProvider;

        public YammerService(IOauthSessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

        public string GetMessages(IDictionary parameters)
        {
            var request = _sessionProvider.GetOAuthSession().Request().Get();
            if(parameters != null) request = request.WithQueryParameters(parameters);
            request = request.ForUrl(@"https://www.yammer.com/api/v1/messages.json");
            return request.ToString();
        }

        public string PostMessage(IDictionary parameters)
        {
            return _sessionProvider.GetOAuthSession().Request().Post().WithFormParameters(parameters).ForUrl(@"https://www.yammer.com/api/v1/messages.json").ToString();
        }
    }
}
