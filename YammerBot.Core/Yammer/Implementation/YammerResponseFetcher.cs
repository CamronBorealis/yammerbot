using System.Collections.Generic;
using YammerBot.Core.Yammer.Interface;

namespace YammerBot.Core.Yammer.Implementation
{
    public class YammerResponseFetcher:IYammerResponseFetcher
    {
        private IDictionary<string, IList<string>> allResponses = new Dictionary<string, IList<string>>
                                   {
                                       {
                                           "yammer bot", new List<string>
                                                             {
                                                                 "Who, me?",
                                                                 "Sorry? What was that?",
                                                                 "Yammer Bot is my name, don't wear it out..."
                                                             }
                                       },
                                       {
                                           "racist", new List<string>
                                                             {
                                                                 "Let's leave the racism out of this...",
                                                                 "Racist? Who's racist?",
                                                                 "Racism isn't cool. Laundry is the only thing that should be separated by color."
                                                             }
                                       },
                                       {
                                           "youtube.com/watch", new List<string>
                                                             {
                                                                 "Sweet vid, brah",
                                                                 "Thanks for sharing",
                                                                 "I just LOL'd. Hard.",
                                                                 "That video is HIGHlarious!"
                                                             }
                                       },
                                       {
                                           "has #joined the DevFacto network", new List<string>
                                                             {
                                                                 "Welcome! I'm Yammer Bot, an automated program that responds to posts. I am maintained by Camron Bute, and am hosted on GitHub at https://github.com/CamronBute/yammerbot. See you around!"
                                                             }
                                       },
                                       {
                                           "munting", new List<string>
                                                             {
                                                                 "Munting, eh? Sounds like someone's thirsty!"
                                                             }
                                       },
                                   };

        public IDictionary<string, IList<string>> GetResponses()
        {
            return allResponses;
        }
    }
}
