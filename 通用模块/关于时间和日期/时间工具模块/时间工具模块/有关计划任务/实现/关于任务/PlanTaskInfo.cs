using System.Collections.Generic;
using System.IOFrancis.FileSystem;
using System.Linq;

namespace System.Time.Plan
{
    /// <summary>
    /// 这个类型是<see cref="IPlanTaskInfo"/>的实现，
    /// 封装了注册计划任务所需要的信息
    /// </summary>
    public record PlanTaskInfo : IPlanTaskInfo
    {
        #region 计划任务的名称
        public string Name { get; }
        #endregion
        #region 对计划任务的描述
        public string? Describe { get; init; }
        #endregion
        #region 计划任务的触发器
        IEnumerable<IPlanTrigger> IPlanTaskInfo.Triggers
           => Triggers;

        /// <summary>
        /// 获取计划任务的所有触发器
        /// </summary>
        public IList<IPlanTrigger> Triggers { get; }
        #endregion
        #region 是否允许任务唤醒硬件
        public bool CanAwaken { get; }
        #endregion
        #region 执行计划任务时的操作
        IEnumerable<(PathText Path, string? Parameters)> IPlanTaskInfo.Start
            => Start;

        /// <summary>
        /// 这个集合枚举执行计划任务时，
        /// 要启动的进程路径，以及传递给进程的参数
        /// </summary>
        public IList<(PathText Path, string? Parameters)> Start { get; }
        #endregion
        #region 构造函数
        #region 直接创建
        /// <summary>
        /// 使用指定的名称，触发器和启动操作初始化对象
        /// </summary>
        /// <param name="Name">计划任务的名称</param>
        /// <param name="Triggers">计划任务的触发器</param>
        /// <param name="Start">这个集合枚举执行计划任务时，
        /// 要启动的进程路径，以及传递给进程的参数</param>
        public PlanTaskInfo(string Name, IEnumerable<IPlanTrigger> Triggers, IEnumerable<(PathText Path, string? Parameters)> Start)
        {
            this.Name = Name;
            this.Triggers = Triggers.ToList();
            this.Start = Start.ToList();
        }
        #endregion
        #region 直接创建，且仅有一个触发器和启动操作
        /// <summary>
        /// 使用指定的名称，触发器和启动操作初始化对象
        /// </summary>
        /// <param name="Name">计划任务的名称</param>
        /// <param name="Trigger">计划任务的触发器</param>
        /// <param name="StartPath">执行计划任务时要启动的进程路径</param>
        /// <param name="StartParameters">执行计划任务时传递给待启动进程的参数</param>
        public PlanTaskInfo(string Name, IPlanTrigger Trigger, PathText StartPath, string? StartParameters = null)
            : this(Name, new[] { Trigger }, new[] { (StartPath, StartParameters) })
        {

        }
        #endregion
        #endregion
    }
}
