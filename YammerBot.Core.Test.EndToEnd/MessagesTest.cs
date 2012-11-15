using System.Collections;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Ninject;
using YammerBot.Core.Yammer.Interface;
using YammerBot.Entity.Yammer;

namespace YammerBot.Core.Test.EndToEnd
{
    [TestFixture]
    class MessagesTest : YammerBotCoreTestBase
    {
        [Test]
        public void ExecuteReplies_GetsNoNewMessages_ShouldSucceed()
        {
            var samplesServiceText = @"{
   ""messages"":[
      {
          ""id"":234423177,
          ""body"":{
              ""plain"":""Welcome!""
          }
      },
      {
          ""id"":234423125,
          ""body"":{
              ""plain"":""Welcome!""
          }
      },
      {
          ""id"":234422180,
          ""body"":{
              ""plain"":""Welcome Jeff!""
          }
      }
   ]
}";
            var messagesInDatabase = new List<Message>
                                         {
                                             new Message{ID = 234423177, Body = new MessageBody{Plain = "Welcome!"}},
                                             new Message{ID = 234423125, Body = new MessageBody{Plain = "Welcome!"}},
                                             new Message{ID = 234422180, Body = new MessageBody{Plain = "Welcome Jeff!"}},
                                         };

            _yammerDatabase.SetupGet(s => s.Messages).Returns(messagesInDatabase);
            _yammerService.Setup(s => s.GetMessages(It.IsAny<IDictionary>())).Returns(samplesServiceText);

            var runner = _kernel.Get<IYammerTaskRunner>();
            runner.ExecuteReplies();

            _yammerDatabase.Verify(v=>v.AddMessage(It.IsAny<Message>()), Times.Never());
            _yammerService.Verify(v=>v.PostMessage(It.IsAny<IDictionary>()), Times.Never());
            _yammerService.Verify(v=>v.GetMessages(It.IsAny<IDictionary>()), Times.Once());
        }

        [Test]
        public void ExecuteReplies_GetsOneNewMessageThatIsNotMatch_ShouldSucceed()
        {
            var samplesServiceText = @"{
   ""messages"":[
      {
          ""id"":234423177,
          ""body"":{
              ""plain"":""Welcome!""
          }
      },
      {
          ""id"":234423125,
          ""body"":{
              ""plain"":""Welcome!""
          }
      },
      {
          ""id"":234422180,
          ""body"":{
              ""plain"":""Welcome Jeff!""
          }
      }
   ]
}";
            var messagesInDatabase = new List<Message>
                                         {
                                             new Message{ID = 234423125, Body = new MessageBody{Plain = "Welcome!"}},
                                             new Message{ID = 234422180, Body = new MessageBody{Plain = "Welcome Jeff!"}},
                                         };

            _yammerDatabase.SetupGet(s => s.Messages).Returns(messagesInDatabase);
            _yammerService.Setup(s => s.GetMessages(It.IsAny<IDictionary>())).Returns(samplesServiceText);

            var runner = _kernel.Get<IYammerTaskRunner>();
            runner.ExecuteReplies();

            _yammerDatabase.Verify(v => v.AddMessage(It.IsAny<Message>()), Times.Once());
            _yammerService.Verify(v => v.PostMessage(It.IsAny<IDictionary>()), Times.Never());
            _yammerService.Verify(v => v.GetMessages(It.IsAny<IDictionary>()), Times.Once());
        }

        [Test]
        public void ExecuteReplies_GetsOneNewMessageWithOneResponseMatch_ShouldSucceed()
        {
            var sampleServiceText = @"{
   ""messages"":[
      {
          ""id"":234423177,
          ""body"":{
              ""plain"":""Welcome racist!""
          }
      },
      {
          ""id"":234423125,
          ""body"":{
              ""plain"":""Welcome!""
          }
      },
      {
          ""id"":234422180,
          ""body"":{
              ""plain"":""Welcome Jeff!""
          }
      }
   ]
}";
            const string samplePostMessageResponse = @"{
   ""messages"":[
      {
          ""id"":{0},
          ""body"":{
              ""plain"":""{1}""
          }
      }
   ]
}";
            var messagesInDatabase = new List<Message>
                                         {
                                             new Message{ID = 234423125, Body = new MessageBody{Plain = "Welcome!"}},
                                             new Message{ID = 234422180, Body = new MessageBody{Plain = "Welcome Jeff!"}},
                                         };

            _yammerDatabase.SetupGet(s => s.Messages).Returns(messagesInDatabase);
            _yammerService.Setup(s => s.GetMessages(It.IsAny<IDictionary>())).Returns(sampleServiceText);
            _yammerService.Setup(s => s.PostMessage(It.IsAny<IDictionary>()))
                          .Returns<IDictionary>(args => samplePostMessageResponse.Replace("{0}", "234423200").Replace("{1}",(string)args["body"]));

            var runner = _kernel.Get<IYammerTaskRunner>();
            runner.ExecuteReplies();

            _yammerDatabase.Verify(v => v.AddMessage(It.IsAny<Message>()), Times.Exactly(2));
            _yammerService.Verify(v => v.PostMessage(It.IsAny<IDictionary>()), Times.Once());
            _yammerService.Verify(v => v.GetMessages(It.IsAny<IDictionary>()), Times.Once());
        }
    }
}
