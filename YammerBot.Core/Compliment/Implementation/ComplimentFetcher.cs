using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using YammerBot.Core.Compliment.Interface;
using YammerBot.Core.System.Interface;

namespace YammerBot.Core.Compliment.Implementation
{
    public class ComplimentFetcher : IComplimentFetcher
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private readonly IComplimentService _complimentService;
        private readonly IComplimentTextTransformer _complimentTextTransformer;
        private readonly IComplimentTextDeserializer _complimentTextDeserializer;

        public ComplimentFetcher(IRandomNumberGenerator randomNumberGenerator, IComplimentService complimentService, IComplimentTextTransformer complimentTextTransformer,
            IComplimentTextDeserializer complimentTextDeserializer)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _complimentService = complimentService;
            _complimentTextTransformer = complimentTextTransformer;
            _complimentTextDeserializer = complimentTextDeserializer;
        }

        public string GetRandomComplimentPhrase()
        {
            var complimentServiceResult = _complimentService.GetComplimentsScriptText();
            var complimentJsonArrayText = _complimentTextTransformer.TransformComplimentTextToJsonArrayText(complimentServiceResult);
            var complimentList = _complimentTextDeserializer.GetComplimentsFromComplimentText(complimentJsonArrayText);
            var randomizedComplimentIndex = _randomNumberGenerator.GetRandomInt32(0, complimentList.Count());
            return complimentList[randomizedComplimentIndex].Phrase;
        }
    }
}
