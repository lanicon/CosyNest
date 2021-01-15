﻿using static System.ExceptionIntervalOut;

namespace System
{
    /// <summary>
    /// 这个记录表示一天之内的某个时间
    /// </summary>
    public record TimeOfDay
    {
        #region 用来描述时间的属性
        #region 小时
        private readonly int HoursField;

        /// <summary>
        /// 获取小时数
        /// </summary>
        public int Hours
        {
            get => HoursField;
            init
            {
                Check(0, 23, value);
                HoursField = value;
            }
        }
        #endregion
        #region 分钟
        private readonly int MinutesField;

        /// <summary>
        /// 获取分钟数
        /// </summary>
        public int Minutes
        {
            get => MinutesField;
            init
            {
                Check(0, 59, value);
                MinutesField = value;
            }
        }
        #endregion
        #region 秒
        private readonly int SecondsField;

        /// <summary>
        /// 获取秒数
        /// </summary>
        public int Seconds
        {
            get => SecondsField;
            init
            {
                Check(0, 59, value);
                SecondsField = value;
            }
        }
        #endregion
        #region 毫秒
        private readonly int MillisecondsField;

        /// <summary>
        /// 获取毫秒数
        /// </summary>
        public int Milliseconds
        {
            get => MillisecondsField;
            init
            {
                Check(0, 999);
                MillisecondsField = value;
            }
        }
        #endregion
        #endregion
        #region 返回距离午夜的时间
        /// <summary>
        /// 返回一个<see cref="TimeSpan"/>，
        /// 它代表该时间距离当日午夜的时间间隔
        /// </summary>
        public TimeSpan DistanceMidnight
            => new TimeSpan(0, Hours, Minutes, Seconds, Milliseconds);
        #endregion
        #region 解构时间
        /// <summary>
        /// 将时间解构为时，分，秒，毫秒数
        /// </summary>
        /// <param name="Hours">小时数</param>
        /// <param name="Minutes">分钟数</param>
        /// <param name="Seconds">秒数</param>
        /// <param name="Milliseconds">毫秒数</param>
        public void Deconstruct(out int Hours, out int Minutes, out int Seconds, out int Milliseconds)
        {
            Hours = this.Hours;
            Minutes = this.Minutes;
            Seconds = this.Seconds;
            Milliseconds = this.Milliseconds;
        }
        #endregion
        #region 重写的ToString方法
        public override string ToString()
            => $"{Hours}:{Minutes}:{Seconds}:{Milliseconds}";
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的小时，分钟，秒和毫秒初始化时间
        /// </summary>
        /// <param name="Hours">小时数</param>
        /// <param name="Minutes">分钟数</param>
        /// <param name="Seconds">秒数</param>
        /// <param name="Milliseconds">毫秒数</param>
        public TimeOfDay(int Hours, int Minutes, int Seconds, int Milliseconds)
        {
            this.Hours = Hours;
            this.Minutes = Minutes;
            this.Seconds = Seconds;
            this.Milliseconds = Milliseconds;
        }
        #endregion
    }
}
