using System.Collections;
using Moq;
using NUnit.Framework;
using YammerBot.Core.Yammer.Implementation;
using YammerBot.Core.Yammer.Interface;

namespace YammerBot.Core.Test.Unit.Yammer
{
    [TestFixture]
    class YammerMessageServiceManagerTest
    {
        private Mock<IYammerService> _service;
        private Mock<IRateGate> _rateGate;

        private IYammerServiceManager _serviceManager;

        [SetUp]
        public void YammerMessageServiceManagerSetUp()
        {
            _service = new Mock<IYammerService>();
            _rateGate = new Mock<IRateGate>();
            _serviceManager = new YammerServiceManager(_service.Object, _rateGate.Object);
        }

        [Test]
        public void YammerMessageServiceManager_GetLatestMessagesTextFromServer_ShouldSucceed()
        {
            _service.Setup(s => s.GetMessages(It.IsAny<IDictionary>())).Returns("Testing");

            var result = _serviceManager.GetLatestMessagesTextFromServer();

            Assert.AreEqual("Testing",result);
            _service.Verify(v=>v.GetMessages(It.Is<IDictionary>(i=>i==null)), Times.Once());
            _rateGate.Verify(v=>v.WaitToProceed(), Times.Once());
        }

        [Test]
        public void YammerMessageServiceManager_GetLatestMessagesTextFromServerAfterMessageID_ShouldSucceed()
        {
            _service.Setup(s => s.GetMessages(It.IsAny<IDictionary>())).Returns("Testing");

            var result = _serviceManager.GetLatestMessagesTextFromServerAfterMessageID(1);

            Assert.AreEqual("Testing", result);
            _service.Verify(v => v.GetMessages(It.Is<IDictionary>(i => (string)(i["older_than"]) == "1")), Times.Once());
            _rateGate.Verify(v => v.WaitToProceed(), Times.Once());
        }

        [Test]
        public void YammerMessageServiceManager_PostNewMessage_ShouldSucceed()
        {
            _service.Setup(s => s.PostMessage(It.IsAny<IDictionary>())).Returns("Testing");

            var result = _serviceManager.PostNewMessage("Yo");

            Assert.AreEqual("Testing", result);
            _service.Verify(v => v.PostMessage(It.Is<IDictionary>(i => (string)(i["body"]) == "Yo")), Times.Once());
            _rateGate.Verify(v => v.WaitToProceed(), Times.Once());
        }

        [Test]
        public void YammerMessageServiceManager_PostMessageReply_ShouldSucceed()
        {
            _service.Setup(s => s.PostMessage(It.IsAny<IDictionary>())).Returns("Testing");

            var result = _serviceManager.PostMessageReply("Yo", 1);

            Assert.AreEqual("Testing", result);
            _service.Verify(v => v.PostMessage(It.Is<IDictionary>(i => (string)(i["body"]) == "Yo" && (string)(i["replied_to_id"]) == "1")), Times.Once());
            _rateGate.Verify(v => v.WaitToProceed(), Times.Once());
        }
    }
}
