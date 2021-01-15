using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static System.ExceptionIntervalOut;

namespace System.Time.Plan
{
    /// <summary>
    /// 表示一个按月份重复的计划任务触发器
    /// </summary>
    sealed class PlanTriggerMonth : PlanTriggerDate, IPlanTriggerMonth
    {
        #region 每几月重复一次
        public int IntervalMonth { get; }
        #endregion
        #region 每个月的第几天重复
        public IEnumerable<int> Days { get; }
        #endregion
        #region 重写的ToString方法
        public override string ToString()
            => ToStringAided($"每{IntervalMonth}月的{Days.Join("、")}日{Time}执行一次，");
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Days">枚举在每个月的第几天重复计划任务</param>
        /// <param name="Time">计划任务在一天内的什么时间执行</param>
        /// <param name="Count">重复执行计划任务的次数，
        /// 如果为<see langword="null"/>，代表执行无数次</param>
        /// <param name="CreateDate">计划任务的创建日期，
        /// 如果为<see langword="null"/>，则使用当前日期</param>
        /// <param name="IntervalMonth">指定每隔几月重复一次</param>
        public PlanTriggerMonth(IEnumerable<int> Days, TimeOfDay Time, int? Count = null, DateTimeOffset? CreateDate = null, int IntervalMonth = 1)
            : base(Time, Count, CreateDate)
        {
            Check(1, null, IntervalMonth);
            this.IntervalMonth = IntervalMonth;
            this.Days = Days.ToHashSet();
            Days.AnyCheck(nameof(Days));
            this.Days.ForEach(x => Check(1, 31, x));
        }
        #endregion
    }
}
