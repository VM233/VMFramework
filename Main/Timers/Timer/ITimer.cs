using VMFramework.Core;

namespace VMFramework.Timers
{
    /// <summary>
    /// 实现此接口的类都可以加到<see cref="TimerManager"/>或<see cref="LogicTickTimerManager"/>中定时。
    /// </summary>
    /// <seealso cref="Timer{TValue}"/>
    /// <seealso cref="TimerManager"/>
    /// <seealso cref="LogicTickTimerManager"/>
    public interface ITimer<TValue> : IGenericPriorityQueueNode<TValue>
    {
        public void OnStart(TValue startedTime, TValue expectedTime)
        {
            
        }

        public void OnTimed()
        {
            
        }

        public void OnStopped(TValue stoppedTime)
        {
            
        }
    }
}