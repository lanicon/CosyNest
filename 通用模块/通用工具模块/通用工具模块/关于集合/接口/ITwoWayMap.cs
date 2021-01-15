using System;

namespace System.Collections.Generic
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以作为一个双向映射表
    /// </summary>
    /// <typeparam name="A">要映射的第一个对象类型</typeparam>
    /// <typeparam name="B">要映射的第二个对象类型</typeparam>
    public interface ITwoWayMap<A, B>
        where A : notnull
        where B : notnull
    {
        #region 获取映射的值
        #region 将A映射为B
        #region 会引发异常
        /// <summary>
        /// 将A对象映射为B对象
        /// </summary>
        /// <param name="a">要映射的A对象</param>
        /// <returns></returns>
        B this[A a] { get; }
        #endregion
        #region 不会引发异常
        /// <summary>
        /// 将A对象映射为B对象，
        /// 如果不存在此映射，不会引发异常
        /// </summary>
        /// <param name="Key">要映射的A对象</param>
        /// <param name="NoFound">如果不存在此映射，则通过这个延迟对象返回一个默认值</param>
        /// <returns></returns>
        (bool Exist, B? Value) TryGetValue(A Key, LazyPro<B>? NoFound = null);
        #endregion
        #endregion
        #region 将B映射为A
        #region 会引发异常
        /// <summary>
        /// 将B对象映射为A对象
        /// </summary>
        /// <param name="b">要映射的B对象</param>
        /// <returns></returns>
        A this[B b] { get; }
        #endregion
        #region 不会引发异常
        /// <summary>
        /// 将B对象映射为A对象，
        /// 如果不存在此映射，不会引发异常
        /// </summary>
        /// <param name="Key">要映射的B对象</param>
        /// <param name="NoFound">如果不存在此映射，则通过这个延迟对象返回一个默认值</param>
        /// <returns></returns>
        (bool Exist, A? Value) TryGetValue(B Key, LazyPro<A>? NoFound = null);
        #endregion
        #endregion
        #endregion
        #region 注册映射
        #region 注册双向映射
        /// <summary>
        /// 注册一个双向映射，双向映射指的是：
        /// 两个对象的映射只能是一对一的，
        /// 而且能够通过任意一个找到另一个
        /// </summary>
        /// <param name="Map">这个元组的两个项会互相映射</param>
        void RegisteredTwo(params (A a, B b)[] Map);
        #endregion
        #region 注册从A到B的单向映射
        /// <summary>
        /// 注册从A到B的单向映射，单向映射指的是：
        /// 只能通过A找到B，但是这个映射可以是一对多的
        /// </summary>
        /// <param name="To">传入下个参数中的任意一个A对象，
        /// 都会映射到这个B对象</param>
        /// <param name="From">通过本集合的任意一个A对象，
        /// 都可以找到上个参数的B对象</param>
        void RegisteredOne(B To, params A[] From);
        #endregion
        #region 注册从B到A的单向映射
        /// <summary>
        /// 注册从B到A的单项映射，单项映射指的是：
        /// 只能通过B找到A，但是这个映射可以是一对多的
        /// </summary>
        /// <param name="To">传入下个参数中的任意一个B对象，
        /// 都会映射到这个A对象</param>
        /// <param name="From">通过本集合的任意一个B对象，
        /// 都可以找到上个参数的A对象</param>
        void RegisteredOne(A To, params B[] From);
        #endregion
        #endregion
    }
}
