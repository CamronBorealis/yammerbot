using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using YammerBot.Core.Yammer.Implementation;
using YammerBot.Core.Yammer.Interface;
using YammerBot.Entity.Yammer;

namespace YammerBot.Core.Test.Unit.Yammer
{
    [TestFixture]
    class YammerMessagePosterTest
    {
        private IYammerMessagePoster _messagePoster;
        private Mock<IYammerServiceManager> _serviceManager;
        private Mock<IYammerMessageResponseDeserializer> _deserializer;

        [SetUp]
        public void YammerMessagePosterSetup()
        {
            _serviceManager = new Mock<IYammerServiceManager>();
            _deserializer = new Mock<IYammerMessageResponseDeserializer>();
            _messagePoster = new YammerMessagePoster(_serviceManager.Object, _deserializer.Object);
        }

        [Test]
        public void YammerMessagePoster_PostMessage_ShouldSucceed()
        {
            var testBody = "This is a body";
            var serviceResponseText = @"Testing";
            var testMessage = new Message();
            var deserializedResponse = new MessagesFetchResponse
                                           {
                                               Messages = new List<Message>
                                                              {
                                                                  testMessage
                                                              }
                                           };

            _serviceManager.Setup(s => s.PostNewMessage(It.IsAny<string>())).Returns(serviceResponseText);
            _deserializer.Setup(s => s.DeserializeMessagesResponse(It.IsAny<string>())).Returns(deserializedResponse);

            var result = _messagePoster.PostMessage(testBody);

            Assert.AreEqual(testMessage, result);
            _serviceManager.Verify(v=>v.PostNewMessage(It.Is<string>(i=>i==testBody)), Times.Once());
            _deserializer.Verify(v=>v.DeserializeMessagesResponse(It.Is<string>(i=>i==serviceResponseText)), Times.Once());
        }

        [Test]
        public void YammerMessagePoster_PostReply_ShouldSucceed()
        {
            var testBody = "This is a body";
            var serviceResponseText = @"Testing";
            var testMessage = new Message();
            var deserializedResponse = new MessagesFetchResponse
            {
                Messages = new List<Message>
                                                              {
                                                                  testMessage
                                                              }
            };

            _serviceManager.Setup(s => s.PostMessageReply(It.IsAny<string>(), It.IsAny<long>())).Returns(serviceResponseText);
            _deserializer.Setup(s => s.DeserializeMessagesResponse(It.IsAny<string>())).Returns(deserializedResponse);

            var result = _messagePoster.PostReply(testBody, 1);

            Assert.AreEqual(testMessage, result);
            _serviceManager.Verify(v => v.PostMessageReply(It.Is<string>(i => i == testBody), It.Is<long>(i=>i==1)), Times.Once());
            _deserializer.Verify(v => v.DeserializeMessagesResponse(It.Is<string>(i => i == serviceResponseText)), Times.Once());
        }
    }
}
