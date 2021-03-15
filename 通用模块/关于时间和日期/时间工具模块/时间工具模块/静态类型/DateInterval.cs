﻿using DI = System.IInterval<System.DateTimeOffset>;

namespace System.Time
{
    /// <summary>
    /// 有关时间区间的工具类
    /// </summary>
    public static class DateInterval
    {
        #region 辅助方法
        /// <summary>
        /// 返回时间区间的辅助方法
        /// </summary>
        /// <param name="Begin">时间区间的开始</param>
        /// <param name="AddDay">指定要在区间的开始加上多少天</param>
        /// <returns></returns>
        private static DI Aided(DateTimeOffset Begin, int AddDay)
            => IInterval.Create(Begin, Begin + new TimeSpan(AddDay, 23, 59, 59, 999));
        #endregion
        #region 返回某天的开始和结束
        /// <summary>
        /// 返回某一天的开始和结束
        /// </summary>
        /// <param name="date">待返回区间的日期，
        /// 如果为<see langword="null"/>，则使用当前日期</param>
        /// <returns>一个时间区间，它从当天的0点开始，到23点59分59秒结束</returns>
        public static DI IntervalDay(DateTimeOffset? date = null)
        {
            var d = date ?? DateTimeOffset.Now;
            var Begin = new DateTimeOffset(d.Date, d.Offset);
            return Aided(Begin, 0);
        }
        #endregion
        #region 返回某一周的开始和结束
        /// <summary>
        /// 返回某一周的开始和结束
        /// </summary>
        /// <param name="date">待返回区间的日期，
        /// 如果为<see langword="null"/>，则使用当前日期</param>
        /// <param name="StartingMonday">如果这个值为<see langword="true"/>，代表一周从周一开始，
        /// 否则代表从周日开始</param>
        /// <returns>一个时间区间，它从一周的第一天0点开始，到一周最后一天23点59分59秒结束</returns>
        public static DI IntervalWeek(DateTimeOffset? date = null, bool StartingMonday = true)
        {
            var d = date ?? DateTimeOffset.Now;
            var weekBegin = StartingMonday ? DayOfWeek.Monday : DayOfWeek.Sunday;
            var Offset = d.DayOfWeek == weekBegin ? 0 :
                d.DayOfWeek.IntervalDay(weekBegin).Last;
            var Begin = new DateTimeOffset(d.Date + TimeSpan.FromDays(Offset), d.Offset);
            return Aided(Begin, 6);
        }
        #endregion
        #region 返回某月的开始和结束
        /// <summary>
        /// 返回某一月的开始和结束
        /// </summary>
        /// <param name="date">待返回区间的日期，
        /// 如果为<see langword="null"/>，则使用当前日期</param>
        /// <returns>一个时间区间，它从当月1号0点开始，
        /// 到当月最后一天23点59分59秒结束</returns>
        public static DI IntervalMonth(DateTimeOffset? date = null)
        {
            var d = date ?? DateTimeOffset.Now;
            var Begin = new DateTimeOffset(new DateTime(d.Year, d.Month, 1), d.Offset);
            var day = DateTime.DaysInMonth(d.Year, d.Month) - 1;
            return Aided(Begin, day);
        }
        #endregion
        #region 返回某年的开始和结束
        /// <summary>
        /// 返回某一年的开始和结束
        /// </summary>
        /// <param name="date">待返回区间的日期，
        /// 如果为<see langword="null"/>，则使用当前日期</param>
        /// <returns>一个时间区间，它从当年1月1号零点开始，
        /// 到当年12月31号23点59分59秒结束</returns>
        public static DI IntervalYear(DateTimeOffset? date = null)
        {
            var d = date ?? DateTimeOffset.Now;
            var Begin = new DateTimeOffset(new DateTime(d.Year, 1, 1), d.Offset);
            var day = DateTime.IsLeapYear(d.Year) ? 365 : 364;
            return Aided(Begin, day);
        }
        #endregion
    }
}
