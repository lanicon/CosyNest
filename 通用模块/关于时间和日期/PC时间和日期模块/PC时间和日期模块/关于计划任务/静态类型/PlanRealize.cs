using System;
using System.Collections.Generic;
using System.Linq;
using System.Maths;

using TaskScheduler;

using static System.Time.Plan.CreatePlan;

namespace System.Time.Plan
{
    /// <summary>
    /// 这个静态类可以用来帮助实现计划任务
    /// </summary>
    static class PlanRealize
    {
        #region 将COM触发器转换为.Net触发器
        /// <summary>
        /// 将COM触发器<see cref="ITrigger"/>转换为<see cref="IPlanTrigger"/>，
        /// 如果触发器类型未能识别，则返回<see langword="null"/>
        /// </summary>
        /// <param name="Trigger">待转换的COM触发器</param>
        /// <returns></returns>
        public static IPlanTrigger? ToTrigger(this ITrigger Trigger)
        {
            #region 用来获取开始时间的本地函数
            static DateTimeOffset Get(ITrigger t)
                => t.StartBoundary.To<DateTimeOffset>();
            #endregion
            #region 用来获取位域的本地函数
            static IEnumerable<int> Flag(int Num, int MaxIndex)
                => ToolBit.AllFlag(Num, MaxIndex).Select(x => x.Index);
            #endregion
            switch (Trigger)
            {
                case IBootTrigger _:
                    return TriggerStart();
                case ITimeTrigger t:
                    return TriggerDisposable(Get(t));
                case IWeeklyTrigger t:
                    var WeekDay = Get(t);
                    return TriggerWeek
                        (Flag(t.DaysOfWeek, 6).Cast<DayOfWeek>(),
                        WeekDay.TimeOfDay(), null, WeekDay, t.WeeksInterval);
                case IDailyTrigger t:
                    return TriggerTimeSpan(Get(t), TimeSpan.FromDays(t.DaysInterval));
                case IMonthlyTrigger t:
                    var MonthlyDate = Get(t);
                    return TriggerYears(MonthlyDate.TimeOfDay(),
                        Flag(t.MonthsOfYear, 11).Select(x => (Month)(x + 1)),
                        Flag(t.DaysOfMonth, 30).Select(x => x + 1),
                        null, MonthlyDate);
                default:
                    return null;
            }
        }
        #endregion
        #region 通过.Net触发器创建COM触发器
        /// <summary>
        /// 通过.Net触发器创建COM触发器
        /// </summary>
        /// <param name="collection">COM触发器所在的集合</param>
        /// <param name="trigger">被转换的.Net触发器</param>
        public static void CreateTrigger(ITriggerCollection collection, IPlanTrigger trigger)
        {
            #region 用于创建触发器的本地函数
            Ret Create<Ret>(_TASK_TRIGGER_TYPE2 type, IPlanTriggerTiming tri)
                where Ret : ITrigger
            {
                var t = collection.Create(type);
                t.StartBoundary = tri.NextDate()!.Value.ToString("O");
                return (Ret)t;
            }
            #endregion
            switch (trigger)
            {
                case IPlanTriggerStart _:
                    collection.Create(_TASK_TRIGGER_TYPE2.TASK_TRIGGER_BOOT); break;
                case IPlanTriggerTimeSpan { Interval: null } t:
                    Create<ITrigger>(_TASK_TRIGGER_TYPE2.TASK_TRIGGER_TIME, t); break;
                case IPlanTriggerTimeSpan t:
                    var Interval = t.Interval!.Value;
                    if (Interval.Modulus(TimeSpan.FromDays(1)) != TimeSpan.Zero)
                        throw new ArgumentException($"传入的{nameof(TimeSpan)}为{Interval}，它不是一天的整数倍");
                    Create<IDailyTrigger>(_TASK_TRIGGER_TYPE2.TASK_TRIGGER_DAILY, t).
                       DaysInterval = (short)Interval.Days; break;
                case IPlanTriggerWeek t:
                    var WeekTrigger = Create<IWeeklyTrigger>(_TASK_TRIGGER_TYPE2.TASK_TRIGGER_WEEKLY, t);
                    WeekTrigger.WeeksInterval = (short)t.IntervalWeek;
                    WeekTrigger.DaysOfWeek = (short)ToolBit.CreateFlag(t.Weeks.Cast<int>().ToArray()); break;
                case IPlanTriggerYears t:
                    var YearsTrigger = Create<IMonthlyTrigger>(_TASK_TRIGGER_TYPE2.TASK_TRIGGER_MONTHLY, t);
                    YearsTrigger.DaysOfMonth = ToolBit.CreateFlag(t.Days.Select(x => x - 1).ToArray());
                    YearsTrigger.MonthsOfYear = (short)ToolBit.CreateFlag(t.Months.Select(x => (int)x - 1).ToArray()); break;
                default:
                    throw new ExceptionTypeUnlawful(trigger,
                        typeof(IPlanTriggerStart), typeof(IPlanTriggerTimeSpan),
                        typeof(IPlanTriggerWeek), typeof(IPlanTriggerYears));
            }
        }
        #endregion
    }
}
