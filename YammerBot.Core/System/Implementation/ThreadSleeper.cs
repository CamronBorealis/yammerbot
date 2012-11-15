using System.Threading;
using YammerBot.Core.System.Interface;

namespace YammerBot.Core.System.Implementation
{
    public class ThreadSleeper:ISleeper
    {
        public void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }
    }
}
