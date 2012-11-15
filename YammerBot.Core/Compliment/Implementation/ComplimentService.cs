using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using YammerBot.Core.Compliment.Interface;

namespace YammerBot.Core.Compliment.Implementation
{
    public class ComplimentService:IComplimentService
    {
        public string GetComplimentsScriptText()
        {
            using (var client = new WebClient())
            {
                var result = client.DownloadString(new Uri(@"http://emergencycompliment.com/js/compliments.js"));
                return result;
            }
        }
    }
}
