using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using YammerBot.Core.System.Interface;
using YammerBot.Core.Yammer.Implementation;
using YammerBot.Core.Yammer.Interface;
using YammerBot.Entity.Yammer;
using System.Linq;

namespace YammerBot.Core.Test.Unit.Yammer
{
    [TestFixture]
    class YammerMessageFetcherTest
    {
        private IYammerMessageFetcher _messageFetcher;
        private Mock<IYammerServiceManager> _service;
        private Mock<IYammerMessageResponseDeserializer> _deserializer;
        private Mock<IYammerMessageDatabaseManager> _databaseManager;
        private Mock<IFileDataProvider> _fileDataProvider;

        [SetUp]
        public void YammerMessageFetcherSetUp()
        {
            _service = new Mock<IYammerServiceManager>();
            _deserializer = new Mock<IYammerMessageResponseDeserializer>();
            _databaseManager = new Mock<IYammerMessageDatabaseManager>();
            _fileDataProvider = new Mock<IFileDataProvider>();
            _messageFetcher = new YammerMessageFetcher(_service.Object, _deserializer.Object,
                _databaseManager.Object, _fileDataProvider.Object);
        }

        [Test]
        public void YammerMessageFetcher_GetLatestMessages_ShouldSucceed()
        {
            var responseText = "Testing";
            var responseMessages = new List<Message>
                                       {
                                           new Message(),
                                           new Message(),
                                           new Message()
                                       };
            var responseObject = new MessagesFetchResponse
                                     {
                                         Messages = responseMessages
                                     };

            _service.Setup(s => s.GetLatestMessagesTextFromServer()).Returns(responseText);
            _deserializer.Setup(s => s.DeserializeMessagesResponse(It.IsAny<string>())).Returns(responseObject);

            var result = _messageFetcher.GetLatestMessages();

            Assert.AreEqual(responseMessages, result);
            _service.Verify(v=>v.GetLatestMessagesTextFromServer(), Times.Once());
            _deserializer.Verify(v=>v.DeserializeMessagesResponse(It.Is<string>(i=>i==responseText)), Times.Once());
        }

        [Test]
        public void YammerMessageFetcher_GetLatestMessagesAndSaveToDisk_ShouldSucceed()
        {
            var responseText = "Testing";
            var responseMessages = new List<Message>
                                       {
                                           new Message(),
                                           new Message(),
                                           new Message()
                                       };
            var responseObject = new MessagesFetchResponse
            {
                Messages = responseMessages
            };

            _service.Setup(s => s.GetLatestMessagesTextFromServer()).Returns(responseText);
            _deserializer.Setup(s => s.DeserializeMessagesResponse(It.IsAny<string>())).Returns(responseObject);

            var result = _messageFetcher.GetLatestMessagesAndSaveToDisk(@"c:\stuff");

            Assert.AreEqual(responseMessages, result);
            _service.Verify(v => v.GetLatestMessagesTextFromServer(), Times.Once());
            _fileDataProvider.Verify(v => v.WriteAllText(It.Is<string>(i => i == @"c:\stuff"), It.Is<string>(i => i == responseText)), Times.Once());
            _deserializer.Verify(v => v.DeserializeMessagesResponse(It.Is<string>(i => i == responseText)), Times.Once());
        }

        [Test]
        public void YammerMessageFetcher_GetUnprocessedMessages_ShouldNotFetchMoreMessages()
        {
            var firstResponseText = "Testing";
            var firstResponseMessages = new List<Message>
                                       {
                                           new Message{ID = 3},
                                           new Message{ID = 2},
                                           new Message{ID = 1}
                                       };
            var firstResponseObject = new MessagesFetchResponse
            {
                Messages = firstResponseMessages
            };

            _service.Setup(s => s.GetLatestMessagesTextFromServer()).Returns(firstResponseText);
            _deserializer.Setup(s => s.DeserializeMessagesResponse(It.Is<string>(i => i == firstResponseText))).Returns(firstResponseObject);
            _databaseManager.Setup(s => s.IsMessageInDatabase(It.Is<long>(i => i > 1))).Returns(false);
            _databaseManager.Setup(s => s.IsMessageInDatabase(It.Is<long>(i => i == 1))).Returns(true);

            var result = _messageFetcher.GetUnprocessedMessages();

            Assert.AreEqual(2, result.Count());
            _service.Verify(v => v.GetLatestMessagesTextFromServer(), Times.Once());
            _service.Verify(v => v.GetLatestMessagesTextFromServerAfterMessageID(It.IsAny<long>()), Times.Never());
            _deserializer.Verify(v => v.DeserializeMessagesResponse(It.Is<string>(i => i == firstResponseText)), Times.Once());
            _databaseManager.Verify(v => v.SaveMessages(It.Is<IEnumerable<Message>>(i => 2 == i.Count())), Times.Once());
            _databaseManager.Verify(v=>v.SaveChanges(), Times.Once());
        }

        [Test]
        public void YammerMessageFetcher_GetUnprocessedMessages_ShouldFetchMoreMessagesWhenNecessary()
        {
            var firstResponseText = "Testing";
            var secondResponseText = "Testing2";
            var firstResponseMessages = new List<Message>
                                       {
                                           new Message{ID = 6},
                                           new Message{ID = 5},
                                           new Message{ID = 4}
                                       };
            var secondResponseMessages = new List<Message>
                                       {
                                           new Message{ID = 3},
                                           new Message{ID = 2},
                                           new Message{ID = 1}
                                       };
            var firstResponseObject = new MessagesFetchResponse
            {
                Messages = firstResponseMessages
            };
            var secondResponseObject = new MessagesFetchResponse
            {
                Messages = secondResponseMessages
            };

            _service.Setup(s => s.GetLatestMessagesTextFromServer()).Returns(firstResponseText);
            _service.Setup(s=>s.GetLatestMessagesTextFromServerAfterMessageID(It.IsAny<long>())).Returns(secondResponseText);
            _deserializer.Setup(s => s.DeserializeMessagesResponse(It.Is<string>(i=>i==firstResponseText))).Returns(firstResponseObject);
            _deserializer.Setup(s => s.DeserializeMessagesResponse(It.Is<string>(i=>i==secondResponseText))).Returns(secondResponseObject);
            _databaseManager.Setup(s => s.IsMessageInDatabase(It.Is<long>(i => i > 1))).Returns(false);
            _databaseManager.Setup(s => s.IsMessageInDatabase(It.Is<long>(i => i == 1))).Returns(true);

            var result = _messageFetcher.GetUnprocessedMessages();

            Assert.AreEqual(5, result.Count());
            _service.Verify(v => v.GetLatestMessagesTextFromServer(), Times.Once());
            _service.Verify(v => v.GetLatestMessagesTextFromServerAfterMessageID(It.Is<long>(i => i == 4)), Times.Once());
            _deserializer.Verify(v => v.DeserializeMessagesResponse(It.Is<string>(i => i == firstResponseText)), Times.Once());
            _deserializer.Verify(v => v.DeserializeMessagesResponse(It.Is<string>(i => i == secondResponseText)), Times.Once());
            _databaseManager.Verify(v => v.SaveMessages(It.Is<IEnumerable<Message>>(i => 5 == i.Count())), Times.Once());
            _databaseManager.Verify(v => v.SaveChanges(), Times.Once());
        }
    }
}
