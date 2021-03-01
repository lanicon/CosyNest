using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace System.Time
{
    /// <summary>
    /// 这个类型是一个按照固定周期执行的定时器
    /// </summary>
    class TimerSimple : ITimer
    {
        #region 封装的对象
        #region 重复次数
        /// <summary>
        /// 定时器的重复次数，
        /// 如果为<see langword="null"/>，代表无限重复
        /// </summary>
        private int? Repeat { get; set; }
        #endregion
        #region 计时器
        /// <summary>
        /// 获取封装的计时器，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private Timer? PackTimer { get; set; }
        #endregion
        #region 定时器的间隔
        /// <summary>
        /// 获取定时器触发的间隔
        /// </summary>
        private TimeSpan Interval { get; }
        #endregion
        #endregion
        #region 接口实现
        #region 计时器到期触发的事件
        public event Action? Due;
        #endregion 
        #region 销毁计时器
        public void Dispose()
            => PackTimer?.Dispose();
        #endregion 
        #region 启动计时器
        public void Start()
        {
            if (PackTimer is null)
            {
                PackTimer = new Timer(x =>
                  {
                      Due?.Invoke();
                      if (Repeat is { } && --Repeat <= 0)
                          Dispose();
                  }, null, TimeSpan.Zero, Interval);
            }
        }
        #endregion 
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Interval">定时器触发的间隔</param>
        /// <param name="Repeat">指定定时器应该重复多少次，
        /// 如果为<see langword="null"/>，代表无限重复</param>
        public TimerSimple(TimeSpan Interval, int? Repeat = 1)
        {
            if (Repeat != null)
                ExceptionIntervalOut.Check(1, null, Repeat.Value);
            this.Interval = Interval;
            this.Repeat = Repeat;
        }
        #endregion
    }
}
