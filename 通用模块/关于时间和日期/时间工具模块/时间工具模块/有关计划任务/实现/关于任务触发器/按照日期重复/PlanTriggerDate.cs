namespace System.Time.Plan
{
    /// <summary>
    /// 表示一个按照日期重复的触发器
    /// </summary>
    abstract class PlanTriggerDate : PlanTriggerTiming, IPlanTriggerDate
    {
        #region 计划任务的创建日期
        public DateTimeOffset CreateDate { get; }
        #endregion
        #region 计划任务的执行时间
        public TimeOfDay Time { get; }
        #endregion
        #region 获取下一次执行计划任务的日期
        public override DateTimeOffset? NextDate(DateTimeOffset? Date = null)
        {
            var date = Date ?? DateTimeOffset.MinValue;
            var begin = CreateDate.ReplaceTime(Time);
            return date <= begin ? begin : throw new NotSupportedException("此API尚未实现");
        }
        #endregion
        #region 重写ToString的辅助方法
        /// <summary>
        /// 重写<see cref="object.ToString(string)"/>的辅助方法，
        /// 封装了派生类文本化中不变的部分
        /// </summary>
        /// <param name="Middle">字符串的中间部分</param>
        /// <returns></returns>
        protected string ToStringAided(string Middle)
            => $"从{CreateDate}开始，" + Middle +
            $"共执行{Count?.ToString() ?? "无限"}次";
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Time">计划任务在一天内的什么时间执行</param>
        /// <param name="Count">重复执行计划任务的次数，
        /// 如果为<see langword="null"/>，代表执行无数次</param>
        /// <param name="CreateDate">计划任务的创建日期，
        /// 如果为<see langword="null"/>，则使用当前日期</param>
        public PlanTriggerDate(TimeOfDay Time, int? Count = null, DateTimeOffset? CreateDate = null)
            : base(Count)
        {
            this.Time = Time;
            this.CreateDate = CreateDate ?? DateTime.Now;
        }
        #endregion
    }
}
