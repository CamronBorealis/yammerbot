using System;

namespace YammerBot.Core.Scheduling.Interface
{
    public interface ILoopingTaskRunner
    {
        void AddTask(int millisecondsBetweenExecutes, Action executeAction);
    }
}