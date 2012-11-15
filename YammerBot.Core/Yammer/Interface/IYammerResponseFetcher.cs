using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YammerBot.Core.Yammer.Interface
{
    public interface IYammerResponseFetcher
    {
        IDictionary<string, IList<string>> GetResponses();
    }
}
