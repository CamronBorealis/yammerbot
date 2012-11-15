using System;
using System.Collections.Generic;
using YammerBot.Core.Compliment.Interface;
using YammerBot.Core.Quote.Interface;
using YammerBot.Core.System.Interface;
using YammerBot.Core.Yammer.Interface;
using YammerBot.Entity.Yammer;

namespace YammerBot.Core.Yammer.Implementation
{
    public class YammerCommandFetcher : IYammerCommandFetcher
    {

        private IRandomNumberGenerator _randomNumberGenerator;
        private IYammerMessagePoster _poster;
        private IQuoteRetriever _quoteRetriever;
        private readonly IComplimentFetcher _complimentFetcher;
        private IDictionary<string, Func<Message, Message>> _allCommands;

        public YammerCommandFetcher(IRandomNumberGenerator randomNumberGenerator, IYammerMessagePoster poster, IQuoteRetriever quoteRetriever, IComplimentFetcher complimentFetcher)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _poster = poster;
            _quoteRetriever = quoteRetriever;
            _complimentFetcher = complimentFetcher;
            _allCommands = new Dictionary<string, Func<Message, Message>>
                                  {
                                      {
                                          "randomquote", (message) =>
                                                             {
                                                                 string quote = null;
                                                                 try
                                                                 {
                                                                     quote = _quoteRetriever.GetNextQuote();
                                                                 }
                                                                 finally
                                                                 {
                                                                     if (quote == null)
                                                                         quote = "I don't have any more quotes...";
                                                                 }
                                                                 return _poster.PostReply(quote, message.ID);
                                                             }
                                      },
                                      {
                                          "tellmetofuckoff", (message) =>
                                                                 {
                                                                     return _poster.PostReply("Fuck off", message.ID);
                                                                 }
                                      },
                                      {
                                          "help", (message) =>
                                                      {
                                                          return _poster.PostReply(
                                                              "@@RandomQuote gives a random quote, @@TellMeToFuckOff tells you to fuck off, @@CatFacts will send you a random cat fact",
                                                              message.ID);
                                                      }
                                      },
                                      {
                                          "catfacts", (message) =>
                                                          {
                                                              var responses = new Dictionary<int, string>
                                                                                  {
                                                                                      {0, "Cats have 9 lives"},
                                                                                      {1, "Cats are cute"},
                                                                                      {2, "Cats just don't give a fuck"}
                                                                                  };
                                                              var selectedResponse =
                                                                  responses[_randomNumberGenerator.GetRandomInt32(0, 3)];
                                                              return _poster.PostReply(selectedResponse, message.ID);
                                                          }
                                      },
                                      {
                                          "whispersweetnothingstome", (message) =>
                                                          {
                                                              return _poster.PostReply(_complimentFetcher.GetRandomComplimentPhrase(), message.ID);
                                                          }
                                      },
                                  };
        }

        public IDictionary<string, Func<Message, Message>> GetCommands()
        {
            return _allCommands;
        }
    }
}
