using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using YammerBot.Core.Compliment.Implementation;
using YammerBot.Core.Compliment.Interface;
using YammerBot.Core.System.Interface;

namespace YammerBot.Core.Test.Unit.Compliment
{
    [TestFixture]
    class ComplimentFetcherTest
    {
        private IComplimentFetcher _complimentFetcher;

        private Mock<IRandomNumberGenerator> _randomNumberGenerator;
        private Mock<IComplimentService> _complimentService;
        private Mock<IComplimentTextTransformer> _complimentTextTransformer;
        private Mock<IComplimentTextDeserializer> _complimentTextDeserializer;

        [SetUp]
        public void ComplimentFetcherSetup()
        {
            _randomNumberGenerator = new Mock<IRandomNumberGenerator>();
            _complimentService = new Mock<IComplimentService>();
            _complimentTextTransformer = new Mock<IComplimentTextTransformer>();
            _complimentTextDeserializer = new Mock<IComplimentTextDeserializer>();
            _complimentFetcher = new ComplimentFetcher(_randomNumberGenerator.Object, _complimentService.Object,
                _complimentTextTransformer.Object, _complimentTextDeserializer.Object);
        }

        [Test]
        public void ComplimentFetcher_GetRandomComplimentPhrase_GetRandomCompliment()
        {
            var complimentServiceResponseText = @"var compliments = [
                                          {
                                            ""color"":""#0080FF"",
                                            ""phrase"":""Your prom date still thinks about you all the time."",
                                            ""link"":""http://society6.com/emergencycompliment/Your-Prom-Date_Print""
                                          },
                                          {
                                            ""color"":""#01DF3A"",
                                            ""phrase"":""All your friends worry they aren\u2019t as funny as you."",
                                            ""link"":""http://society6.com/emergencycompliment/As-Funny-As-You-BSA_Print""
                                          },
                                          {
                                            ""color"":""#0080FF"",
                                            ""phrase"":""Your boss loved that thing you did at work today."",
                                            ""link"":""http://society6.com/emergencycompliment/Your-Boss-Loved-That_Print""
                                          }
                                        ];";
            var transformedText = @"[
                                      {
                                        ""color"":""#0080FF"",
                                        ""phrase"":""Your prom date still thinks about you all the time."",
                                        ""link"":""http://society6.com/emergencycompliment/Your-Prom-Date_Print""
                                      },
                                      {
                                        ""color"":""#01DF3A"",
                                        ""phrase"":""All your friends worry they aren\u2019t as funny as you."",
                                        ""link"":""http://society6.com/emergencycompliment/As-Funny-As-You-BSA_Print""
                                      },
                                      {
                                        ""color"":""#0080FF"",
                                        ""phrase"":""Your boss loved that thing you did at work today."",
                                        ""link"":""http://society6.com/emergencycompliment/Your-Boss-Loved-That_Print""
                                      }
                                    ]";
            var complimentList = new List<Entity.Compliments.Compliment>
                                     {
                                         new Entity.Compliments.Compliment{Phrase = "Your prom date still thinks about you all the time."},
                                         new Entity.Compliments.Compliment{Phrase = "All your friends worry they aren\u2019t as funny as you."},
                                         new Entity.Compliments.Compliment{Phrase = "Your boss loved that thing you did at work today."},
                                     };
            _randomNumberGenerator.Setup(s => s.GetRandomInt32(It.IsAny<int>(), It.IsAny<int>())).Returns(1);
            _complimentService.Setup(s => s.GetComplimentsScriptText()).Returns(complimentServiceResponseText);
            _complimentTextTransformer.Setup(s => s.TransformComplimentTextToJsonArrayText(It.IsAny<string>())).Returns(transformedText);
            _complimentTextDeserializer.Setup(s => s.GetComplimentsFromComplimentText(It.IsAny<string>())).Returns(complimentList);

            var randomCompliment = _complimentFetcher.GetRandomComplimentPhrase();
            Assert.AreEqual("All your friends worry they aren\u2019t as funny as you.", randomCompliment);

            _randomNumberGenerator.Verify(v => v.GetRandomInt32(It.Is<int>(i => i == 0), It.Is<int>(i => i == 3)), Times.Once());
            _complimentService.Verify(v => v.GetComplimentsScriptText(), Times.Once());
            _complimentTextTransformer.Verify(v=>v.TransformComplimentTextToJsonArrayText(It.Is<string>(i=>i==complimentServiceResponseText)), Times.Once());
            _complimentTextDeserializer.Verify(v=>v.GetComplimentsFromComplimentText(It.Is<string>(i=>i==transformedText)), Times.Once());
        }
    }
}
