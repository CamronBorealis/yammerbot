using System;
using YammerBot.Core.Scheduling.Interface;
using YammerBot.Core.System.Interface;

namespace YammerBot.Core.Scheduling.Implementation
{
    public class LoopingTaskRunner:ILoopingTaskRunner
    {
        private readonly ISleeper _sleeper;

        public LoopingTaskRunner(ISleeper sleeper)
        {
            _sleeper = sleeper;
        }

        public void AddTask(int millisecondsBetweenExecutes, Action executeAction)
        {
            while (true)
            {
                try
                {
                    executeAction();
                }
                catch(Exception ex){Console.WriteLine(ex.Message);}
                //_sleeper.Sleep(millisecondsBetweenExecutes);
            }
        }
    }
}
