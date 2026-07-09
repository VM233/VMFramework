namespace VMFramework.Timers
{
    /// <summary>
    /// 以现实时间为基准的定时器管理器。
    /// 如果需要以Tick为基准的计时管理，请使用<see cref="LogicTickTimerManager"/>
    /// </summary>
    /// <seealso cref="LogicTickTimerManager"/>
    /// <seealso cref="ITimer{T}"/>
    /// <seealso cref="Timer"/>
    public interface ITimerManager
    {
        public double CurrentTime { get; }
        
        /// <summary>
        /// 添加一个定时器到队列中，并指定延迟。
        /// Adds a timer to the queue with a delay.
        /// 一般复杂度为O(log n)
        /// </summary>
        public void Add(ITimer<double> timer, float delay);

        /// <summary>
        /// <para>从队列中移除一个定时器。Removes a timer from the queue.</para>
        /// <para>一般复杂度为O(log n)</para>
        /// <para>如果定时器不在队列中，会抛出异常。
        /// 建议在调用此方法前使用Contains()方法。
        /// 或者使用<see cref="TryStop"/>方法安全地移除定时器。
        /// if the timer is not in the queue, it will throw an exception.
        /// So it's better to use Contains() before calling this method.
        /// Or use <see cref="TryStop"/> to safely remove a timer.</para>
        /// </summary>
        public void Stop(ITimer<double> timer);

        /// <summary>
        /// 检查一个定时器是否在队列中。Checks if a timer is in the queue.
        /// 一般复杂度为O(1)。
        /// </summary>
        public bool Contains(ITimer<double> timer);

        /// <summary>
        /// 尝试停止一个定时器，<see cref="Stop"/>的安全版本。
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        public bool TryStop(ITimer<double> timer);

        /// <seealso cref="TryStop"/>
        /// <seealso cref="Add"/>
        public bool TryStopAndAdd(ITimer<double> timer, float delay);
    }
}