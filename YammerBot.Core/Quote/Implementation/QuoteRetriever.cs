using System.Linq;
using YammerBot.Core.Quote.Interface;
using YammerBot.Core.System.Interface;

namespace YammerBot.Core.Quote.Implementation
{
    public class QuoteRetriever : IQuoteRetriever
    {
        private readonly IEnvironmentInfoProvider _environmentInfoProvider;
        private readonly IFileDataProvider _fileDataProvider;

        public QuoteRetriever(IEnvironmentInfoProvider environmentInfoProvider, IFileDataProvider fileDataProvider)
        {
            _environmentInfoProvider = environmentInfoProvider;
            _fileDataProvider = fileDataProvider;
        }

        public string GetNextQuote()
        {
            var filePath = _environmentInfoProvider.DataDirectory + @"\Quotes.txt";
            var lines = _fileDataProvider.ReadAllLines(filePath).ToList();
            if (lines.Any()) _fileDataProvider.WriteAllLines(filePath, lines.Skip(1));
            return lines.Any()?lines.First():null;
        }
    }
}
