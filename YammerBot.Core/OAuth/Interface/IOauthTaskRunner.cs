using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YammerBot.Core.OAuth.Interface
{
    public interface IOauthTaskRunner
    {
        string GetAuthLink();
    }
}
