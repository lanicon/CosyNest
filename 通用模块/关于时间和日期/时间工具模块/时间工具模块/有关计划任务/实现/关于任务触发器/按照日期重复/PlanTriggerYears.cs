using System.Collections.Generic;
using System.Linq;

namespace System.Time.Plan
{
    /// <summary>
    /// 表示一个按照年度重复的计划任务触发器
    /// </summary>
    class PlanTriggerYears : PlanTriggerDate, IPlanTriggerYears
    {
        #region 枚举月份的第几天重复
        public IEnumerable<int> Days { get; }
        #endregion
        #region 枚举在几月重复
        public IEnumerable<Month> Months { get; }
        #endregion
        #region 重写的ToString方法
        public override string ToString()
            => ToStringAided($"在{Months.Select(x => (int)x).Join("、")}月的{Days.Join("、")}日{Time}执行一次，");
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Time">计划任务在一天内的什么时间执行</param>
        /// <param name="Months">枚举在几月份重复计划任务</param>
        /// <param name="Days">枚举在月份的第几天重复计划任务</param>
        /// <param name="Count">重复执行计划任务的次数，
        /// 如果为<see langword="null"/>，代表执行无数次</param>
        /// <param name="CreateDate">计划任务的创建日期，
        /// 如果为<see langword="null"/>，则使用当前日期</param>
        public PlanTriggerYears(TimeOfDay Time, IEnumerable<Month> Months, IEnumerable<int> Days, int? Count = null, DateTimeOffset? CreateDate = null)
            : base(Time, Count, CreateDate)
        {
            this.Months = Months.ToHashSet();
            this.Days = Days.ToHashSet();
            Days.AnyCheck(nameof(Days));
            Months.AnyCheck(nameof(Months));
            Days.ForEach(x => ExceptionIntervalOut.Check(1, 31, x));
        }
        #endregion
    }
}
