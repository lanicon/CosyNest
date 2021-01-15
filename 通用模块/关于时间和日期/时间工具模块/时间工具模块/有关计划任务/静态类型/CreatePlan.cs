using System.Collections.Generic;

namespace System.Time.Plan
{
    /// <summary>
    /// 这个静态类可以用来创建和计划任务有关的对象
    /// </summary>
    public static class CreatePlan
    {
        #region 创建触发器
        #region 在硬件启动时执行
        private static readonly IPlanTriggerStart TriggerStartField =
            new PlanTriggerStart();
        /// <summary>
        /// 创建一个触发器，它在硬件启动时执行计划任务
        /// </summary>
        /// <returns></returns>
        public static IPlanTriggerStart TriggerStart()
            => TriggerStartField;
        #endregion
        #region 仅执行一次
        /// <summary>
        /// 创建一个触发器，它仅在指定时间执行一次
        /// </summary>
        /// <param name="Begin">触发器的执行时间</param>
        /// <returns></returns>
        public static IPlanTriggerTimeSpan TriggerDisposable(DateTimeOffset Begin)
            => new PlanTriggerTimeSpan(Begin, null, 1);
        #endregion
        #region 按照指定时间间隔执行
        /// <summary>
        /// 创建一个触发器，它按照指定的<see cref="TimeSpan"/>间隔重复执行任务
        /// </summary>
        /// <param name="Begin">第一次执行计划任务的时间</param>
        /// <param name="Interval">重复执行计划任务的间隔，
        /// 如果为<see langword="null"/>，代表不会重复</param>
        /// <param name="Count">重复执行计划任务的次数，
        /// 如果为<see langword="null"/>，代表执行无数次</param>
        public static IPlanTriggerTimeSpan TriggerTimeSpan(DateTimeOffset Begin, TimeSpan? Interval, int? Count = null)
            => new PlanTriggerTimeSpan(Begin, Interval, Count);
        #endregion
        #region 按照星期执行
        /// <summary>
        /// 创建一个触发器，它按照星期重复执行计划任务
        /// </summary>
        /// <param name="Weeks">枚举计划任务在星期几执行</param>
        /// <param name="Time">计划任务在一天内的什么时间执行</param>
        /// <param name="Count">重复执行计划任务的次数，
        /// 如果为<see langword="null"/>，代表执行无数次</param>
        /// <param name="CreateDate">计划任务的创建日期，
        /// 如果为<see langword="null"/>，则使用当前日期</param>
        /// <param name="IntervalWeek">指定每隔几周重复一次</param>
        public static IPlanTriggerWeek TriggerWeek(IEnumerable<DayOfWeek> Weeks, TimeOfDay Time, int? Count = null, DateTimeOffset? CreateDate = null, int IntervalWeek = 1)
            => new PlanTriggerWeek(Weeks, Time, Count, CreateDate, IntervalWeek);
        #endregion
        #region 按照月份重复执行
        /// <summary>
        /// 创建一个触发器，它按照月份重复执行计划任务
        /// </summary>
        /// <param name="Days">枚举在每个月的第几天重复计划任务</param>
        /// <param name="Time">计划任务在一天内的什么时间执行</param>
        /// <param name="Count">重复执行计划任务的次数，
        /// 如果为<see langword="null"/>，代表执行无数次</param>
        /// <param name="CreateDate">计划任务的创建日期，
        /// 如果为<see langword="null"/>，则使用当前日期</param>
        /// <param name="IntervalMonth">指定每隔几月重复一次</param>
        public static IPlanTriggerMonth TriggerMonth(IEnumerable<int> Days, TimeOfDay Time, int? Count = null, DateTimeOffset? CreateDate = null, int IntervalMonth = 1)
            => new PlanTriggerMonth(Days, Time, Count, CreateDate, IntervalMonth);
        #endregion
        #region 按照年度执行
        /// <summary>
        /// 创建一个触发器，它按照年度重复执行计划任务
        /// </summary>
        /// <param name="Time">计划任务在一天内的什么时间执行</param>
        /// <param name="Months">枚举在几月份重复计划任务</param>
        /// <param name="Days">枚举在月份的第几天重复计划任务</param>
        /// <param name="Count">重复执行计划任务的次数，
        /// 如果为<see langword="null"/>，代表执行无数次</param>
        /// <param name="CreateDate">计划任务的创建日期，
        /// 如果为<see langword="null"/>，则使用当前日期</param>
        /// <returns></returns>
        public static IPlanTriggerYears TriggerYears(TimeOfDay Time, IEnumerable<Month> Months, IEnumerable<int> Days, int? Count = null, DateTimeOffset? CreateDate = null)
            => new PlanTriggerYears(Time, Months, Days, Count, CreateDate);
        #endregion
        #endregion
    }
}
