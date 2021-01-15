﻿using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 有关进程与线程的工具类
    /// </summary>
    public static class ExtenTask
    {
        #region 关于ValueTask
        #region 从Task转换
        /// <summary>
        /// 将<see cref="Task"/>转换为<see cref="ValueTask"/>
        /// </summary>
        /// <param name="Task">待转换的<see cref="Task"/></param>
        /// <returns></returns>
        public static ValueTask ToValueTask(this Task Task)
            => new(Task);
        #endregion
        #region 等待ValueTask
        /// <summary>
        /// 等待一个<see cref="ValueTask"/>
        /// </summary>
        /// <param name="task">待等待的<see cref="ValueTask"/></param>
        public static void Wait(this ValueTask task)
            => task.AsTask().Wait();
        #endregion
        #endregion
        #region 关于ValueTask<T>
        #region 从Task<T>转换
        /// <summary>
        /// 将<see cref="Task{TResult}"/>转换为<see cref="ValueTask{TResult}"/>
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="Task">待转换的<see cref="Task{TResult}"/></param>
        /// <returns></returns>
        public static ValueTask<TResult> ToValueTask<TResult>(this Task<TResult> Task)
            => new(Task);
        #endregion
        #region 等待ValueTask<T>
        /// <summary>
        /// 等待一个<see cref="ValueTask{TResult}"/>，
        /// 并获取它的结果
        /// </summary>
        /// <typeparam name="Ret">返回值类型</typeparam>
        /// <param name="task">待等待的<see cref="ValueTask{TResult}"/></param>
        /// <returns></returns>
        public static Ret Result<Ret>(this ValueTask<Ret> task)
            => task.AsTask().Result;
        #endregion
        #endregion
    }
}
