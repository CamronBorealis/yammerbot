using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using YammerBot.Core.System.Interface;
using YammerBot.Core.Yammer.Implementation;
using YammerBot.Core.Yammer.Interface;
using YammerBot.Entity.Yammer;

namespace YammerBot.Core.Test.Unit.Yammer
{
    [TestFixture]
    class YammerDatabaseManagerTest
    {
        private IYammerMessageDatabaseManager _databaseManager;
        private Mock<IYammerDatabase> _database;

        [SetUp]
        public void YammerDatabaseManagerSetup()
        {
            _database = new Mock<IYammerDatabase>();
            _databaseManager = new YammerMessageDatabaseManager(_database.Object);
        }

        [Test]
        public void YammerDatabaseManager_SaveMessages_ShouldSucceed()
        {
            var testMessages = new List<Message>
                                   {
                                       new Message(),
                                       new Message(),
                                       new Message()
                                   };

            _databaseManager.SaveMessages(testMessages);

            _database.Verify(v=>v.AddMessage(It.IsAny<Message>()), Times.Exactly(3));
        }

        [Test]
        public void YammerDatabaseManager_IsMessageInDatabase_ShouldFindMessage()
        {
            var testMessages = new List<Message>
                                   {
                                       new Message{ID = 1},
                                   };

            _database.SetupGet(s => s.Messages).Returns(testMessages);

            var result = _databaseManager.IsMessageInDatabase(1);

            Assert.IsTrue(result);
            _database.VerifyGet(v=>v.Messages, Times.Once());
        }

        [Test]
        public void YammerDatabaseManager_IsMessageInDatabase_ShouldNotFindMessage()
        {
            var testMessages = new List<Message>
                                   {
                                       new Message{ID = 1},
                                   };

            _database.SetupGet(s => s.Messages).Returns(testMessages);

            var result = _databaseManager.IsMessageInDatabase(2);

            Assert.IsFalse(result);
            _database.VerifyGet(v => v.Messages, Times.Once());
        }

        [Test]
        public void YammerDatabaseManager_SaveChanges_ShouldSucceed()
        {
            _databaseManager.SaveChanges();

            _database.Verify(v=>v.SaveChanges(), Times.Once());
        }
    }
}
