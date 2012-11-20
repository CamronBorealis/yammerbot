using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using YammerBot.Core.Compliment.Interface;
using YammerBot.Core.Quote.Interface;
using YammerBot.Core.System.Interface;
using YammerBot.Core.Yammer.Implementation;
using YammerBot.Core.Yammer.Interface;
using YammerBot.FunctionalCore;
using YammerBot.Entity.Yammer;

namespace YammerBot.Core.Test.Unit.Yammer
{
    [TestFixture]
    class YammerCommandFetcherTest
    {
        private IYammerCommandFetcher _commandFetcher;
        private Mock<IRandomNumberGenerator> _randomNumberGenerator;
        private Mock<IYammerMessagePoster> _messagePoster;
        private Mock<IQuoteRetriever> _quoteRetriever;
        private Mock<IComplimentFetcher> _complimentFetcher;
        private Mock<IDictionaryService> _dictionaryService;

        [SetUp]
        public void YammerCommandFetcherSetup()
        {
            _randomNumberGenerator = new Mock<IRandomNumberGenerator>();
            _messagePoster = new Mock<IYammerMessagePoster>();
            _quoteRetriever = new Mock<IQuoteRetriever>();
            _complimentFetcher = new Mock<IComplimentFetcher>();
            _dictionaryService = new Mock<IDictionaryService>();
            _commandFetcher = new YammerCommandFetcher(_randomNumberGenerator.Object, _messagePoster.Object, _quoteRetriever.Object, _complimentFetcher.Object, _dictionaryService.Object);

            _dictionaryService.Setup(s => s.GetDefinitions(It.IsAny<string>())).Returns(new List<string>());
            _dictionaryService.Setup(s => s.GetDefinitions("block")).Returns(new List<string>() { "block, n. it's a block!" });
        }

        [Test]
        public void YammerCommandFetcher_GetCommands_RandomQuoteShouldReplyWithNextAvailableQuote()
        {
            var func = _commandFetcher.GetCommands()["randomquote"];
            _quoteRetriever.Setup(s => s.GetNextQuote()).Returns("Testing");
            var result = func(new Message{ID = 1});
            _messagePoster.Verify(v=>v.PostReply(It.Is<string>(i=>i=="Testing"), It.Is<long>(i=>i==1)), Times.Once());
        }

        [Test]
        public void YammerCommandFetcher_GetCommands_RandomQuoteShouldReplyWithMessageWithNoNextQuote()
        {
            var func = _commandFetcher.GetCommands()["randomquote"];
            _quoteRetriever.Setup(s => s.GetNextQuote()).Returns((string)null);
            var result = func(new Message { ID = 1 });
            _messagePoster.Verify(v => v.PostReply(It.Is<string>(i => i == "I don't have any more quotes..."), It.Is<long>(i => i == 1)), Times.Once());
        }

        [Test]
        public void YammerCommandFetcher_GetCommands_TellMeToFuckOffShouldTellYouToFuckOff()
        {
            var func = _commandFetcher.GetCommands()["tellmetofuckoff"];
            _quoteRetriever.Setup(s => s.GetNextQuote()).Returns((string)null);
            var result = func(new Message { ID = 1 });
            _messagePoster.Verify(v => v.PostReply(It.Is<string>(i => i == "Fuck off"), It.Is<long>(i => i == 1)), Times.Once());
        }

        [Test]
        public void YammerCommandFetcher_GetCommands_HelpShouldReturnHelpMessage()
        {
            var func = _commandFetcher.GetCommands()["help"];
            var result = func(new Message { ID = 1 });
            _messagePoster.Verify(v => v.PostReply(It.Is<string>(i => i == "@@RandomQuote gives a random quote, @@TellMeToFuckOff tells you to fuck off, @@CatFacts will send you a random cat fact"), It.Is<long>(i => i == 1)), Times.Once());
        }

        [Test]
        public void YammerCommandFetcher_GetCommands_CatFactsShouldReturnMessage()
        {
            var func = _commandFetcher.GetCommands()["catfacts"];
            var result = func(new Message { ID = 1 });
            _messagePoster.Verify(v => v.PostReply(It.IsAny<string>(), It.Is<long>(i => i == 1)), Times.Once());
        }

        [Test]
        public void YammerCommandFetcher_GetCommands_SexifyShouldReturnSexifiedPhrase()
        {
            var func = _commandFetcher.GetCommands()["sexify"];
            var result = func(new Message { ID = 1, Body = new MessageBody { Plain = "@@sexify I am a block!" } });
            _messagePoster.Verify(v => v.PostReply(It.Is<string>(i => i == "I am a sexy block!"), It.Is<long>(i => i == 1)), Times.Once());
        }
    }
}
