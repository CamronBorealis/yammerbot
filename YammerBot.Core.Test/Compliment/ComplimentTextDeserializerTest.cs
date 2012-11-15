using System.Collections.Generic;
using NUnit.Framework;
using YammerBot.Core.Compliment.Implementation;
using YammerBot.Core.Compliment.Interface;

namespace YammerBot.Core.Test.Unit.Compliment
{
    [TestFixture]
    class ComplimentTextDeserializerTest
    {
        private IComplimentTextDeserializer _complimentTextDeserializer;

        [SetUp]
        public void ComplimentTextDeserializerSetup()
        {
            _complimentTextDeserializer = new ComplimentTextDeserializer();
        }

        [Test]
        public void ComplimentTextDeserializer_GetComplimentsFromComplimentText_ShouldSucceed()
        {
            var complimentText = @"[
  {
    ""color"":""#0080FF"",
    ""phrase"":""Your prom date still thinks about you all the time."",
    ""link"":""http://society6.com/emergencycompliment/Your-Prom-Date_Print""
  },
  {
    ""color"":""#01DF3A"",
    ""phrase"":""All your friends worry they aren\u2019t as funny as you."",
    ""link"":""http://society6.com/emergencycompliment/As-Funny-As-You-BSA_Print""
  }
]";
            var expectedComplimentList = new List<Entity.Compliments.Compliment>
                                             {
                                                 new Entity.Compliments.Compliment
                                                    {Phrase = "Your prom date still thinks about you all the time."},
                                                 new Entity.Compliments.Compliment
                                                    {Phrase ="All your friends worry they aren\u2019t as funny as you."}
                                             };
            var complimentList = _complimentTextDeserializer.GetComplimentsFromComplimentText(complimentText);
            Assert.AreEqual(expectedComplimentList, complimentList);
        }
    }
}
