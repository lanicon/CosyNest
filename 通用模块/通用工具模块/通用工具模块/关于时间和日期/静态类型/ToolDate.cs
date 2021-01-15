﻿using System;

namespace System
{
    /// <summary>
    /// 有关时间和日期的工具类
    /// </summary>
    public static class ToolDate
    {
        #region 计算年龄
        /// <summary>
        /// 计算年龄，未满一年的部分会被舍去
        /// </summary>
        /// <param name="birthday">生日</param>
        /// <param name="now">指定现在的日期，
        /// 如果为<see langword="null"/>，则自动获取</param>
        /// <returns></returns>
        public static int Age(DateTimeOffset birthday, DateTimeOffset? now = null)
        {
            var Now = now ?? DateTimeOffset.Now;
            ExceptionIntervalOut.Check(birthday, null, Now);
            var age = Now.Year - birthday.Year;
            if (birthday.DayOfYear > Now.DayOfYear)             //如果有未满一年的部分，则舍去
                age--;
            return Math.Max(0, age);
        }
        #endregion
    }
}
