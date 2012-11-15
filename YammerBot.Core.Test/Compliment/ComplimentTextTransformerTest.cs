using NUnit.Framework;
using YammerBot.Core.Compliment.Implementation;
using YammerBot.Core.Compliment.Interface;

namespace YammerBot.Core.Test.Unit.Compliment
{
    [TestFixture]
    public class ComplimentTextTransformerTest
    {
        private IComplimentTextTransformer _complimentTextTransformer;

        [SetUp]
        public void ComplimentTextTransformerSetup()
        {
            _complimentTextTransformer = new ComplimentTextTransformer();
        }

        [Test]
        public void ComplimentTextTransformer_TransformComplimentTextToJsonArrayText_ShouldSucceed()
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
  }
];
";
            var expectedTransformResult = @"[
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

            var jsonArrayText = _complimentTextTransformer.TransformComplimentTextToJsonArrayText(complimentServiceResponseText);
            Assert.AreEqual(expectedTransformResult, jsonArrayText);
        }
    }
}
