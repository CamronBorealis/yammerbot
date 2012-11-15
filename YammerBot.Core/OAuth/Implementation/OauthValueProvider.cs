using System.Configuration;
using YammerBot.Core.OAuth.Interface;

namespace YammerBot.Core.OAuth.Implementation
{
    public class OauthValueProvider : IOauthValueProvider
    {
        public string ConsumerKey
        {
            get { return ConfigurationManager.AppSettings["ConsumerKey"]; }
        }
        public string ConsumerSecret
        {
            get { return ConfigurationManager.AppSettings["ConsumerSecret"]; }
        }
        public string Token
        {
            get { return ConfigurationManager.AppSettings["Token"]; }
        }
        public string TokenSecret
        {
            get { return ConfigurationManager.AppSettings["TokenSecret"]; }
        }
    }
}
