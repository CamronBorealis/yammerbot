using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YammerBot.Core.OAuth.Interface;

namespace YammerBot.Core.OAuth.Implementation
{
    public class OAuthTaskRunner:IOauthTaskRunner
    {
        private readonly IOauthSessionProvider _sessionProvider;

        public OAuthTaskRunner(IOauthSessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

        public string GetAuthLink()
        {
            var session = _sessionProvider.GetOAuthSession();
            var token = session.GetRequestToken("POST");
            return session.GetUserAuthorizationUrlForToken(token);
        }
    }
}
