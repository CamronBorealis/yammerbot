using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using YammerBot.Core.System.Interface;
using YammerBot.Core.Yammer.Implementation;
using YammerBot.Core.Yammer.Interface;
using YammerBot.Entity.Yammer;

namespace YammerBot.Core.Test.Unit.Yammer
{
    [TestFixture]
    class YammerTaskRunnerTest
    {
        private IYammerTaskRunner _yammerTaskRunner;
        private Mock<IYammerMessagePoster> _messagePoster;
        private Mock<IYammerMessageFetcher> _messageFetcher;
        private Mock<IYammerMessageDatabaseManager> _databaseManager;
        private Mock<IYammerResponseFetcher> _responseFetcher;
        private Mock<IRandomNumberGenerator> _randomNumberGenerator;
        private Mock<IYammerCommandFetcher> _commandFetcher;

        [SetUp]
        public void YammerTaskRunnerSetup()
        {
            _messagePoster = new Mock<IYammerMessagePoster>();
            _messageFetcher = new Mock<IYammerMessageFetcher>();
            _databaseManager = new Mock<IYammerMessageDatabaseManager>();
            _responseFetcher = new Mock<IYammerResponseFetcher>();
            _randomNumberGenerator = new Mock<IRandomNumberGenerator>();
            _commandFetcher = new Mock<IYammerCommandFetcher>();
            _commandFetcher.Setup(s => s.GetCommands()).Returns(new Dictionary<string, Func<Message, Message>>());
            _yammerTaskRunner = new YammerTaskRunner(_messagePoster.Object, _messageFetcher.Object, _databaseManager.Object,
                _responseFetcher.Object, _randomNumberGenerator.Object, _commandFetcher.Object);
        }

        [Test]
        public void YammerTaskRunner_ExecuteReplies_ShouldDoNothingWhenNoMatchesFound()
        {
            _messageFetcher.Setup(s => s.GetUnprocessedMessages()).Returns(new List<Message>
                                                                          {
                                                                              new Message
                                                                                  {
                                                                                      Body =
                                                                                          new MessageBody
                                                                                              {Plain = "yammer bot"},
                                                                                              ID = 1
                                                                                  }
                                                                          });
            _responseFetcher.Setup(s => s.GetResponses()).Returns(new Dictionary<string, IList<string>>());
            _databaseManager.Setup(s => s.IsMessageInDatabase(It.Is<int>(i => i == 1))).Returns(false);
            _yammerTaskRunner.ExecuteReplies();
            _messagePoster.Verify(v=>v.PostReply(It.IsAny<string>(), It.Is<long>(i=>i==1)), Times.Never());
        }

        [Test]
        public void YammerTaskRunner_ExecuteReplies_ShouldPostFirstReplyWhenMatchingKeyword()
        {
            _messageFetcher.Setup(s => s.GetUnprocessedMessages()).Returns(new List<Message>
                                                                          {
                                                                              new Message
                                                                                  {
                                                                                      Body =
                                                                                          new MessageBody
                                                                                              {Plain = "yammer bot"},
                                                                                              ID = 1
                                                                                  }
                                                                          });
            var responses = new Dictionary<string, IList<string>>
                                {
                                    {"yammer", new List<string> {"Testing1", "Testing2"}}
                                };
            _responseFetcher.Setup(s => s.GetResponses()).Returns(responses);
            _databaseManager.Setup(s => s.IsMessageInDatabase(It.Is<int>(i => i == 1))).Returns(false);
            _yammerTaskRunner.ExecuteReplies();
            _messagePoster.Verify(v => v.PostReply(It.Is<string>(i => i == "Testing1"), It.Is<long>(i => i == 1)), Times.Once());
            _messagePoster.Verify(v => v.PostReply(It.Is<string>(i => i != "Testing1"), It.Is<long>(i => i == 1)), Times.Never());
        }

        [Test]
        public void YammerTaskRunner_ExecuteReplies_ShouldPostSecondReplyWhenMatchingKeywordAndReturningRandomNumber()
        {
            _messageFetcher.Setup(s => s.GetUnprocessedMessages()).Returns(new List<Message>
                                                                          {
                                                                              new Message
                                                                                  {
                                                                                      Body =
                                                                                          new MessageBody
                                                                                              {Plain = "yammer bot"},
                                                                                              ID = 1
                                                                                  }
                                                                          });
            var responses = new Dictionary<string, IList<string>>
                                {
                                    {"yammer", new List<string> {"Testing1", "Testing2"}}
                                };
            _responseFetcher.Setup(s => s.GetResponses()).Returns(responses);
            _randomNumberGenerator.Setup(s => s.GetRandomInt32(It.IsAny<int>(), It.IsAny<int>())).Returns(1);
            _databaseManager.Setup(s => s.IsMessageInDatabase(It.Is<int>(i => i == 1))).Returns(false);
            _yammerTaskRunner.ExecuteReplies();
            _messagePoster.Verify(v => v.PostReply(It.Is<string>(i => i == "Testing2"), It.Is<long>(i => i == 1)), Times.Once());
            _messagePoster.Verify(v => v.PostReply(It.Is<string>(i => i != "Testing2"), It.Is<long>(i => i == 1)), Times.Never());
        }

        [Test]
        public void YammerTaskRunner_ExecuteReplies_ShouldPostReplyRegardlessOfCasing()
        {
            _messageFetcher.Setup(s => s.GetUnprocessedMessages()).Returns(new List<Message>
                                                                          {
                                                                              new Message
                                                                                  {
                                                                                      Body =
                                                                                          new MessageBody
                                                                                              {Plain = "yAmmer bot"},
                                                                                              ID = 1
                                                                                  }
                                                                          });
            var responses = new Dictionary<string, IList<string>>
                                {
                                    {"yaMmer", new List<string> {"Testing1", "Testing2"}}
                                };
            _responseFetcher.Setup(s => s.GetResponses()).Returns(responses);
            _randomNumberGenerator.Setup(s => s.GetRandomInt32(It.IsAny<int>(), It.IsAny<int>())).Returns(1);
            _databaseManager.Setup(s => s.IsMessageInDatabase(It.Is<int>(i => i == 1))).Returns(false);
            _yammerTaskRunner.ExecuteReplies();
            _messagePoster.Verify(v => v.PostReply(It.Is<string>(i => i == "Testing2"), It.Is<long>(i => i == 1)), Times.Once());
            _messagePoster.Verify(v => v.PostReply(It.Is<string>(i => i != "Testing2"), It.Is<long>(i => i == 1)), Times.Never());
        }

        [Test]
        public void YammerTaskRunner_ExecuteReplies_ShouldExecuteCommands()
        {
            _messageFetcher.Setup(s => s.GetUnprocessedMessages()).Returns(new List<Message>
                                                                          {
                                                                              new Message
                                                                                  {
                                                                                      Body =
                                                                                          new MessageBody
                                                                                              {Plain = "@@testcommand"},
                                                                                              ID = 1
                                                                                  }
                                                                          });
            _responseFetcher.Setup(s => s.GetResponses()).Returns(new Dictionary<string, IList<string>>());
            _commandFetcher.Setup(s => s.GetCommands()).Returns(new Dictionary<string, Func<Message, Message>>
                                                                    {
                                                                        {
                                                                            "testcommand",
                                                                            message => { return new Message{ID = 1}; }
                                                                        }
                                                                    });
            _yammerTaskRunner.ExecuteReplies();
            _databaseManager.Verify(s=>s.SaveMessages(It.Is<IEnumerable<Message>>(i=>i.First().ID == 1)));
        }
    }
}
