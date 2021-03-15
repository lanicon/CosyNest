namespace System.Time.Plan
{
    /// <summary>
    /// 表示一个具备确定开始时间的触发器
    /// </summary>
    abstract class PlanTriggerTiming : IPlanTriggerTiming
    {
        #region 获取执行的次数
        public int? Count { get; }
        #endregion
        #region 获取下一次执行计划任务的日期
        public abstract DateTimeOffset? NextDate(DateTimeOffset? Date = null);
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的次数初始化触发器
        /// </summary>
        /// <param name="Count">重复执行计划任务的次数，
        /// 如果为<see langword="null"/>，代表执行无数次</param>
        public PlanTriggerTiming(int? Count = null)
        {
            if (Count != null)
                ExceptionIntervalOut.Check(1, null, Count.Value);
            this.Count = Count;
        }
        #endregion
    }
}
