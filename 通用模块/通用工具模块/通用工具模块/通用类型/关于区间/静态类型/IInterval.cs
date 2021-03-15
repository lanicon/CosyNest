﻿namespace System
{
    /// <summary>
    /// 这个静态类可以用来帮助创建区间
    /// </summary>
    public static class IInterval
    {
        #region 创建区间
        #region 创建抽象区间
        /// <summary>
        /// 使用指定的下限和上限创建区间
        /// </summary>
        /// <typeparam name="Obj">位于区间中的对象，
        /// 它通过<see cref="IComparable{T}"/>进行比较</typeparam>
        /// <param name="Min">区间的下限，
        /// 如果为<see langword="null"/>，代表没有下限</param>
        /// <param name="Max">区间的上限，
        /// 如果为<see langword="null"/>，代表没有上限</param>
        /// <returns></returns>
        public static IInterval<Obj> Create<Obj>(IComparable<Obj>? Min, IComparable<Obj>? Max)
            => new Interval<Obj>
            {
                Min = Min,
                Max = Max
            };
        #endregion
        #region 创建具体区间
        /// <summary>
        /// 使用指定的下限和上限创建区间
        /// </summary>
        /// <typeparam name="Obj">位于区间中的对象</typeparam>
        /// <param name="Min">区间的下限，
        /// 如果为<see langword="null"/>，代表没有下限</param>
        /// <param name="Max">区间的上限，
        /// 如果为<see langword="null"/>，代表没有上限</param>
        /// <returns></returns>
        public static IIntervalSpecific<Obj> Create<Obj>(Obj? Min, Obj? Max)
            where Obj : struct, IComparable<Obj>
            => new IntervalSpecific<Obj>(Min, Max);
        #endregion
        #endregion
        #region 检查对象是否位于区间中
        /// <summary>
        /// 检查一个对象是否位于区间中
        /// </summary>
        /// <typeparam name="Obj">位于区间中的对象，
        /// 它通过<see cref="IComparable{T}"/>进行比较</typeparam>
        /// <param name="obj">待检查的对象</param>
        /// <param name="Min">区间的下限，
        /// 如果为<see langword="null"/>，代表没有下限</param>
        /// <param name="Max">区间的上限，
        /// 如果为<see langword="null"/>，代表没有上限</param>
        /// <returns>一个枚举，它指示对象在区间中的位置</returns>
        public static IntervalPosition CheckInInterval<Obj>(Obj obj, IComparable<Obj>? Min, IComparable<Obj>? Max)
            => Create(Min, Max).CheckInInterval(obj);
        #endregion
    }
}
