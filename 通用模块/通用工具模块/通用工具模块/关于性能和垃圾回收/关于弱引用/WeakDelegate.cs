﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    /// <summary>
    /// 表示一个对目标使用弱引用的委托，可以防止由于事件引起的内存泄露问题
    /// </summary>
    /// <typeparam name="Del">被封装的委托类型</typeparam>
    public class WeakDelegate<Del> : IEnumerable<(WeakReference? Target, MethodInfo Met)>
        where Del : Delegate
    {
        #region 说明文档
        /*说明：
          #实现弱事件的推荐模式：
          用一个初始值为null的私有字段保存WeakDel对象，
          当第一个用户订阅事件时，实例化WeakDel，
          这样的好处是可以只实例化有效的事件，节约内存
           
          静态类ToolPerfo的AddWeakDel方法已经封装了这个操作，
          在注册事件时，请使用这个方法，在移除事件或调用事件时，
          请判断WeakDel是否为null
           
          但如果当类被实例化时，一定会有委托被注册到WeakDel，
          则可以不遵循这个模式
           
          #以下情况下不推荐使用弱事件，而是建议使用.net传统事件：
          注册事件的对象不被其他任何对象引用，
          如果在此时使用弱事件，很可能因为事件源被回收而导致事件无法触发*/
        #endregion
        #region 用弱引用储存的委托
        /// <summary>
        /// 储存委托的集合，委托被简化为一个元组，
        /// 第一个项是方法的调用方，第二个项是方法的元数据
        /// </summary>
        private HashSet<(WeakReference? Target, MethodInfo Met)> DelList { get; }
        = new HashSet<(WeakReference? Target, MethodInfo Met)>();
        #endregion
        #region 关于委托
        #region 动态调用委托列表中的所有委托
        /// <summary>
        /// 动态调用委托列表中的所有委托，在调用之前，
        /// 会自动删除掉所有已经失去目标引用，而且不是静态方法的委托
        /// </summary>
        /// <param name="Parameters">方法调用的参数列表</param>
        public void DynamicInvoke(params object?[] Parameters)
        {
            LinkedList<(WeakReference?, MethodInfo)>? Failure = null;     //如果没有委托被回收，则不初始化这个对象，减轻GC压力
            foreach (var item in DelList)
            {
                if (item is { Met: { IsStatic: false }, Target: { Target: null } })
                {
                    Failure ??= new();
                    Failure.AddLast(item);
                }
                else item.Met.Invoke(item.Target?.Target, Parameters);
            }
            Failure?.ForEach(x => DelList.Remove(x));                       //删除掉所有已经失去目标引用，而且不是静态方法的委托
        }
        #endregion
        #region 对委托进行一个弱引用封装
        /// <summary>
        /// 对委托进行一个弱引用封装
        /// </summary>
        /// <param name="Del">要封装的委托</param>
        /// <returns>一个元组，分别是委托目标的弱引用，以及委托的方法</returns>
        private static (WeakReference?, MethodInfo) PackDel(Del Del)
        {
            var Met = Del.Method;
            return (Met.IsStatic ? null : new WeakReference(Del.Target), Met);      //如果是静态方法，就不封装弱引用，节约内存
        }
        #endregion
        #region 添加委托
        /// <summary>
        /// 向调用列表添加一个委托
        /// </summary>
        /// <param name="Del">要添加的委托</param>
        public void Add(Del Del)
            => DelList.Add(WeakDelegate<Del>.PackDel(Del));
        #endregion
        #region 移除委托
        /// <summary>
        /// 向调用列表中移除委托
        /// </summary>
        /// <param name="Del">要移除的委托</param>
        public void Remove(Del Del)
            => DelList.Remove(WeakDelegate<Del>.PackDel(Del));
        #endregion
        #region 移除所有委托
        /// <summary>
        /// 从调用列表中移除所有委托
        /// </summary>
        public void Clear()
            => DelList.Clear();
        #endregion
        #endregion
        #region 接口实现
        public IEnumerator<(WeakReference? Target, MethodInfo Met)> GetEnumerator()
            => DelList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
        #endregion
    }
}
