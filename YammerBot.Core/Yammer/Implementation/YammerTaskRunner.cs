using System.Collections.Generic;
using System.Linq;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using YammerBot.Core.System.Interface;
using YammerBot.Core.Yammer.Interface;
using YammerBot.Entity.Yammer;

namespace YammerBot.Core.Yammer.Implementation
{
    public class YammerTaskRunner : IYammerTaskRunner
    {
        private readonly IYammerMessagePoster _poster;
        private readonly IYammerMessageFetcher _messageFetcher;
        private readonly IYammerMessageDatabaseManager _databaseManager;
        private readonly IYammerResponseFetcher _responseFetcher;
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private readonly IYammerCommandFetcher _commandFetcher;

        public YammerTaskRunner(IYammerMessagePoster poster, IYammerMessageFetcher messageFetcher, IYammerMessageDatabaseManager databaseManager, IYammerResponseFetcher responseFetcher, IRandomNumberGenerator randomNumberGenerator, IYammerCommandFetcher commandFetcher)
        {
            _poster = poster;
            _messageFetcher = messageFetcher;
            _databaseManager = databaseManager;
            _responseFetcher = responseFetcher;
            _randomNumberGenerator = randomNumberGenerator;
            _commandFetcher = commandFetcher;
        }

        public void GetAccessToken()
        {
            string requestUrl = "https://www.yammer.com/oauth/request_token";
            string userAuthorizeUrl = "https://www.yammer.com/oauth/authorize";
            string accessUrl = "https://www.yammer.com/oauth/access_token";

            var context = new OAuthConsumerContext
            {
                ConsumerKey = @"ZWZ4FiczbUeQbuDyp1JhDg",
                ConsumerSecret = @"14HZ8T9YgIuWQtlaFdhtLLBncWx0ZHbuoioNzdSzrA",
                SignatureMethod = SignatureMethod.PlainText,
                UseHeaderForOAuthParameters = true
            };
            var session = new OAuthSession(context, requestUrl, userAuthorizeUrl, accessUrl);
            var authCode = "";
            var requestToken = session.GetRequestToken("POST");
            var authLink = session.GetUserAuthorizationUrlForToken(requestToken);
            var accessToken = session.ExchangeRequestTokenForAccessToken(requestToken, authCode);
        }

        public void ExecuteReplies()
        {
            var messages = _messageFetcher.GetUnprocessedMessages().ToList();
            ProcessKeywordMatches(messages);
            ProcessCommandMatches(messages);
            _databaseManager.SaveChanges();
        }

        private void ProcessCommandMatches(List<Message> messages)
        {
            var allCommands = _commandFetcher.GetCommands();
            foreach (var key in allCommands.Keys)
            {
                foreach (var message in messages.Where(w => w.Body.Plain.ToLower().Contains("@@" + key)))
                {
                    var newMessage = allCommands[key](message);
                    _databaseManager.SaveMessages(new[] { newMessage });
                }
            }
        }

        private void ProcessKeywordMatches(List<Message> messages)
        {
            var allResponses = _responseFetcher.GetResponses();
            foreach (var key in allResponses.Keys)
            {
                foreach (var message in messages.Where(w => w.Body.Plain.ToLower().Contains(key.ToLower())))
                {
                    var responses = allResponses[key];
                    var selectedResponse = responses[_randomNumberGenerator.GetRandomInt32(0, responses.Count)];
                    var newMessage = _poster.PostReply(selectedResponse, message.ID);
                    _databaseManager.SaveMessages(new[] { newMessage });
                }
            }
        }
    }
}
