using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Ninject;
using YammerBot.Core.Compliment.Implementation;
using YammerBot.Core.Compliment.Interface;
using YammerBot.Core.OAuth.Implementation;
using YammerBot.Core.OAuth.Interface;
using YammerBot.Core.Quote.Implementation;
using YammerBot.Core.Quote.Interface;
using YammerBot.Core.Scheduling.Implementation;
using YammerBot.Core.Scheduling.Interface;
using YammerBot.Core.System.Implementation;
using YammerBot.Core.System.Interface;
using YammerBot.Core.Yammer.Implementation;
using YammerBot.Core.Yammer.Interface;
using YammerBot.FunctionalCore;

namespace YammerBot.Core.Test.EndToEnd
{
    [TestFixture]
    public abstract class YammerBotCoreTestBase
    {
        protected StandardKernel _kernel;

        protected Mock<IEnvironmentInfoProvider> _environmentInfoProvider;
        protected Mock<IFileDataProvider> _fileDataProvider;
        protected Mock<ISleeper> _sleeper;
        protected Mock<IComplimentService> _complimentService;
        protected Mock<IYammerService> _yammerService;
        protected Mock<IOauthValueProvider> _oauthValueProvider;
        protected Mock<IYammerDatabase> _yammerDatabase;
        protected Mock<IDictionaryService> _dictionaryService;

        [SetUp]
        public void YammerBotCoreTestBaseSetUp()
        {
            _environmentInfoProvider = new Mock<IEnvironmentInfoProvider>();
            _fileDataProvider = new Mock<IFileDataProvider>();
            _sleeper = new Mock<ISleeper>();
            _complimentService = new Mock<IComplimentService>();
            _yammerService = new Mock<IYammerService>();
            _oauthValueProvider = new Mock<IOauthValueProvider>();
            _yammerDatabase = new Mock<IYammerDatabase>();
            _dictionaryService = new Mock<IDictionaryService>();

            _kernel = new StandardKernel();
            _kernel.Bind<IYammerMessageResponseDeserializer>().To<YammerMessageResponseDeserializer>();
            _kernel.Bind<IYammerMessageDatabaseManager>().To<YammerMessageDatabaseManager>();
            _kernel.Bind<IYammerMessagePoster>().To<YammerMessagePoster>();
            _kernel.Bind<IYammerMessageFetcher>().To<YammerMessageFetcher>();
            _kernel.Bind<IYammerMessageServiceManager>().To<YammerMessageServiceManager>();
            _kernel.Bind<IYammerTaskRunner>().To<YammerTaskRunner>();
            _kernel.Bind<IQuoteRetriever>().To<QuoteRetriever>();
            _kernel.Bind<ILoopingTaskRunner>().To<LoopingTaskRunner>();
            _kernel.Bind<IYammerResponseFetcher>().To<YammerResponseFetcher>();
            _kernel.Bind<IYammerCommandFetcher>().To<YammerCommandFetcher>();
            _kernel.Bind<IRandomNumberGenerator>().To<RandomNumberGenerator>();
            _kernel.Bind<IComplimentFetcher>().To<ComplimentFetcher>();
            _kernel.Bind<IComplimentTextDeserializer>().To<ComplimentTextDeserializer>();
            _kernel.Bind<IComplimentTextTransformer>().To<ComplimentTextTransformer>();

            _kernel.Bind<IOauthSessionProvider>().To<OauthSessionProvider>();

            _kernel.Bind<IEnvironmentInfoProvider>().ToConstant(_environmentInfoProvider.Object);
            _kernel.Bind<IFileDataProvider>().ToConstant(_fileDataProvider.Object);
            _kernel.Bind<ISleeper>().ToConstant(_sleeper.Object);
            _kernel.Bind<IComplimentService>().ToConstant(_complimentService.Object);
            _kernel.Bind<IYammerService>().ToConstant(_yammerService.Object);
            _kernel.Bind<IOauthValueProvider>().ToConstant(_oauthValueProvider.Object);
            _kernel.Bind<IYammerDatabase>().ToConstant(_yammerDatabase.Object);
            _kernel.Bind<IDictionaryService>().ToConstant(_dictionaryService.Object);
        }
    }
}
