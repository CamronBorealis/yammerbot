using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YammerBot.Core.System.Interface;

namespace YammerBot.Core.System.Implementation
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        public int GetRandomInt32(int min, int max)
        {
            return new Random().Next(min, max);
        }
    }
}
