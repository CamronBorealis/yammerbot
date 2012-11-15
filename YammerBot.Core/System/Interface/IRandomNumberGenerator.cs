using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YammerBot.Core.System.Interface
{
    public interface IRandomNumberGenerator
    {
        int GetRandomInt32(int min, int max);
    }
}
