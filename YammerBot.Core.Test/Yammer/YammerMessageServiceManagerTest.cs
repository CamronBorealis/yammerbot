using System.Collections;
using Moq;
using NUnit.Framework;
using YammerBot.Core.System.Interface;
using YammerBot.Core.Yammer.Implementation;
using YammerBot.Core.Yammer.Interface;

namespace YammerBot.Core.Test.Unit.Yammer
{
    [TestFixture]
    class YammerMessageServiceManagerTest
    {
        private Mock<IYammerService> _service;

        private IYammerMessageServiceManager _messageServiceManager;

        [SetUp]
        public void YammerMessageServiceManagerSetUp()
        {
            _service = new Mock<IYammerService>();
            _messageServiceManager = new YammerMessageServiceManager(_service.Object);
        }

        [Test]
        public void YammerMessageServiceManager_GetLatestMessagesTextFromServer_ShouldSucceed()
        {
            _service.Setup(s => s.GetMessages(It.IsAny<IDictionary>())).Returns("Testing");

            var result = _messageServiceManager.GetLatestMessagesTextFromServer();

            Assert.AreEqual("Testing",result);
            _service.Verify(v=>v.GetMessages(It.Is<IDictionary>(i=>i==null)), Times.Once());
        }

        [Test]
        public void YammerMessageServiceManager_GetLatestMessagesTextFromServerAfterMessageID_ShouldSucceed()
        {
            _service.Setup(s => s.GetMessages(It.IsAny<IDictionary>())).Returns("Testing");

            var result = _messageServiceManager.GetLatestMessagesTextFromServerAfterMessageID(1);

            Assert.AreEqual("Testing", result);
            _service.Verify(v => v.GetMessages(It.Is<IDictionary>(i => (string)(i["older_than"]) == "1")), Times.Once());
        }

        [Test]
        public void YammerMessageServiceManager_PostNewMessage_ShouldSucceed()
        {
            _service.Setup(s => s.PostMessage(It.IsAny<IDictionary>())).Returns("Testing");

            var result = _messageServiceManager.PostNewMessage("Yo");

            Assert.AreEqual("Testing", result);
            _service.Verify(v => v.PostMessage(It.Is<IDictionary>(i => (string)(i["body"]) == "Yo")), Times.Once());
        }

        [Test]
        public void YammerMessageServiceManager_PostMessageReply_ShouldSucceed()
        {
            _service.Setup(s => s.PostMessage(It.IsAny<IDictionary>())).Returns("Testing");

            var result = _messageServiceManager.PostMessageReply("Yo", 1);

            Assert.AreEqual("Testing", result);
            _service.Verify(v => v.PostMessage(It.Is<IDictionary>(i => (string)(i["body"]) == "Yo" && (string)(i["replied_to_id"]) == "1")), Times.Once());
        }
    }
}
