using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using YammerBot.Core.Compliment.Interface;
using YammerBot.Core.Quote.Interface;
using YammerBot.Core.System.Interface;
using YammerBot.Core.Yammer.Interface;
using YammerBot.FunctionalCore;
using YammerBot.Entity.Yammer;

namespace YammerBot.Core.Yammer.Implementation
{
    public class YammerCommandFetcher : IYammerCommandFetcher
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private readonly IYammerMessagePoster _poster;
        private readonly IQuoteRetriever _quoteRetriever;
        private readonly IComplimentFetcher _complimentFetcher;
        private readonly IDictionaryService _dictionaryService;
        private readonly IDictionary<string, Func<Message, Message>> _allCommands;

        public YammerCommandFetcher(IRandomNumberGenerator randomNumberGenerator, 
                                    IYammerMessagePoster poster, 
                                    IQuoteRetriever quoteRetriever, 
                                    IComplimentFetcher complimentFetcher,
                                    IDictionaryService dictionaryService)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _poster = poster;
            _quoteRetriever = quoteRetriever;
            _complimentFetcher = complimentFetcher;
            _dictionaryService = dictionaryService;
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
                                      {
                                          "sexify", (message) =>
                                                          {
                                                              var cmd = new Command(message.Body.Plain);
                                                              var wordDefinitions = cmd.Argument.Split(' ')
                                                                                       .Select(word => WordProcessor.NormalizeWord(word))
                                                                                       .Aggregate(new Dictionary<string,IEnumerable<string>>(),(accum, word) =>
                                                                                                   {
                                                                                                       if (!accum.Keys.Contains(word))
                                                                                                           accum.Add(word, _dictionaryService.GetDefinitions(word));
                                                                                                       return accum;
                                                                                                   });

                                                              return _poster.PostReply(WordProcessor.Sexify(cmd.Argument, wordDefinitions), message.ID);
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
