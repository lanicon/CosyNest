﻿using System.Collections.Generic;
using System.Linq;

namespace System.Time.Plan
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个计划任务面板，
    /// 它可以创建和枚举计划任务
    /// </summary>
    public interface IPlanPanel
    {
        #region 枚举所有计划任务
        /// <summary>
        /// 枚举所有计划任务
        /// </summary>
        IEnumerable<IPlanTask> Tasks { get; }
        #endregion
        #region 按照名称获取计划任务
        /// <summary>
        /// 按照名称获取计划任务，如果找不到，则返回<see langword="null"/>
        /// </summary>
        /// <param name="Name">计划任务的名称</param>
        /// <returns></returns>
        IPlanTask? this[string Name]
            => Tasks.FirstOrDefault(x => x.Name == Name);
        #endregion
        #region 创建计划任务
        /// <summary>
        /// 创建计划任务，并返回被创建的计划任务
        /// </summary>
        /// <param name="Info">这个对象封装了创建计划任务所需的信息</param>
        /// <returns></returns>
        IPlanTask CreatePlan(IPlanTaskInfo Info);
        #endregion
    }
}
