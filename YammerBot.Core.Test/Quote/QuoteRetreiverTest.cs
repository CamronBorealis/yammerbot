using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using YammerBot.Core.Quote.Implementation;
using YammerBot.Core.Quote.Interface;
using YammerBot.Core.System.Interface;

namespace YammerBot.Core.Test.Unit.Quote
{
    [TestFixture]
    class QuoteRetreiverTest
    {
        private IQuoteRetriever _quoteRetriever;
        private Mock<IEnvironmentInfoProvider> _environmentInfoProvider;
        private Mock<IFileDataProvider> _fileDataProvider;

        [SetUp]
        public void QuoteRetreiverSetup()
        {
            _environmentInfoProvider = new Mock<IEnvironmentInfoProvider>();
            _fileDataProvider = new Mock<IFileDataProvider>();
            _quoteRetriever = new QuoteRetriever(_environmentInfoProvider.Object, _fileDataProvider.Object);
        }

        [Test]
        public void QuoteRetreiver_GetNextQuote_UsesCorrectFileName_ShouldSucceed()
        {
            _environmentInfoProvider.Setup(s => s.DataDirectory).Returns(@"c:\SomeDirectory");
            _fileDataProvider.Setup(s => s.ReadAllLines(It.IsAny<string>())).Returns(new List<string>());
            _quoteRetriever.GetNextQuote();
            _fileDataProvider.Verify(v=>v.ReadAllLines(It.Is<string>(i=>i==@"c:\SomeDirectory\Quotes.txt")));
        }

        [Test]
        public void QuoteRetreiver_GetNextQuote_ReturnsFirstLineWhenOneLineExists_ShouldSucceed()
        {
            _fileDataProvider.Setup(s => s.ReadAllLines(It.IsAny<string>())).Returns(new List<string>{"Testing"});
            Assert.AreEqual("Testing",_quoteRetriever.GetNextQuote());
        }

        [Test]
        public void QuoteRetreiver_GetNextQuote_WritesNoDataWhenOneLineExists_ShouldSucceed()
        {
            _fileDataProvider.Setup(s => s.ReadAllLines(It.IsAny<string>())).Returns(new List<string> { "Testing" });
            _quoteRetriever.GetNextQuote();
            _fileDataProvider.Verify(v=>v.WriteAllLines(It.IsAny<string>(), It.Is<IEnumerable<string>>(i=>!i.Any())));
        }

        [Test]
        public void QuoteRetreiver_GetNextQuote_ReturnsFirstLineWhenTwoLinesExists_ShouldSucceed()
        {
            _fileDataProvider.Setup(s => s.ReadAllLines(It.IsAny<string>())).Returns(new List<string> { "Testing", "Testing2" });
            Assert.AreEqual("Testing", _quoteRetriever.GetNextQuote());
        }

        [Test]
        public void QuoteRetreiver_GetNextQuote_WritesOneDataWhenOneLineExists_ShouldSucceed()
        {
            _fileDataProvider.Setup(s => s.ReadAllLines(It.IsAny<string>())).Returns(new List<string> { "Testing", "Testing2" });
            _quoteRetriever.GetNextQuote();
            _fileDataProvider.Verify(v => v.WriteAllLines(It.IsAny<string>(), It.Is<IEnumerable<string>>(i => i.ToList()[0]=="Testing2")));
        }
    }
}
