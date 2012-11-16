﻿using System.Linq;
using Ninject;
using YammerBot.Core.Compliment.Implementation;
using YammerBot.Core.Compliment.Interface;
using YammerBot.Core.OAuth.Implementation;
using YammerBot.Core.OAuth.Interface;
using YammerBot.Core.Quote.Implementation;
using YammerBot.Core.Quote.Interface;
using YammerBot.Core.Scheduling.Implementation;
using YammerBot.Core.Scheduling.Interface;
using YammerBot.Core.Yammer.Implementation;
using YammerBot.Core.Yammer.Interface;
using YammerBot.Core.System.Implementation;
using YammerBot.Core.System.Interface;

namespace YammerBot.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Bind<IYammerMessageResponseDeserializer>().To<YammerMessageResponseDeserializer>();
            kernel.Bind<IYammerMessageDatabaseManager>().To<YammerMessageDatabaseManager>();
            kernel.Bind<IYammerMessagePoster>().To<YammerMessagePoster>();
            kernel.Bind<IYammerMessageFetcher>().To<YammerMessageFetcher>();
            kernel.Bind<IYammerMessageServiceManager>().To<YammerMessageServiceManager>();
            kernel.Bind<IYammerTaskRunner>().To<YammerTaskRunner>();
            kernel.Bind<IQuoteRetriever>().To<QuoteRetriever>();
            kernel.Bind<ILoopingTaskRunner>().To<LoopingTaskRunner>();
            kernel.Bind<IEnvironmentInfoProvider>().To<EnvironmentInfoProvider>();
            kernel.Bind<IFileDataProvider>().To<FileDataProvider>();
            kernel.Bind<ISleeper>().To<ThreadSleeper>();
            kernel.Bind<IYammerResponseFetcher>().To<YammerResponseFetcher>();
            kernel.Bind<IYammerCommandFetcher>().To<YammerCommandFetcher>();
            kernel.Bind<IRandomNumberGenerator>().To<RandomNumberGenerator>();
            kernel.Bind<IComplimentFetcher>().To<ComplimentFetcher>();
            kernel.Bind<IComplimentService>().To<ComplimentService>();
            kernel.Bind<IComplimentTextDeserializer>().To<ComplimentTextDeserializer>();
            kernel.Bind<IComplimentTextTransformer>().To<ComplimentTextTransformer>();
            kernel.Bind<IYammerService>().To<YammerService>();
            kernel.Bind<IOauthValueProvider>().To<OauthValueProvider>();

            kernel.Bind<IOauthSessionProvider>().To<OauthSessionProvider>().InSingletonScope();
            kernel.Bind<IYammerDatabase>().To<YammerDatabase>().InSingletonScope();

            var database = kernel.Get<IYammerDatabase>();
            if (!database.Messages.Any())
            {
                var fetcher = kernel.Get<IYammerMessageFetcher>();
                var databaseManager = kernel.Get<IYammerMessageDatabaseManager>();
                var messages = fetcher.GetLatestMessages();
                databaseManager.SaveMessages(messages);
                databaseManager.SaveChanges();
            }

            var runner = kernel.Get<IYammerTaskRunner>();
            var looper = kernel.Get<ILoopingTaskRunner>();

            looper.AddTask(60000, runner.ExecuteReplies);
        }
    }
}
