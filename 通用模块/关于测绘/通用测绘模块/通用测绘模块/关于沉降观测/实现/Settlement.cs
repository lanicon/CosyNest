using System.Collections.Generic;
using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 这个类型是<see cref="ISettlement"/>的实现，
    /// 可以视为沉降观测中的一站
    /// </summary>
    class Settlement : ISettlement
    {
        #region 名称
        public string Name { get; }
        #endregion
        #region 高程
        public IUTLength High { get; init; }
        #endregion
        #region 闭合站
        public ISettlement? Closed { get; init; }
        #endregion
        #region 闭合差
        public IUTLength? ClosedDifference { get; set; }
        #endregion
        #region 父节点
        public INode? Father { get; }
        #endregion
        #region 子节点
        private LinkedList<INode> SonField { get; } = new();

        public IEnumerable<INode> Son => SonField;
        #endregion
        #region 添加下一站
        public ISettlementObservation Add(string Name, IUTLength Recording)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region 构造函数
#pragma warning disable CS8618
        /// <summary>
        /// 使用指定的沉降点名称初始化对象
        /// </summary>
        /// <param name="Name">沉降观测点的名称</param>
        public Settlement(string Name)
        {
            this.Name = Name;
        }
#pragma warning restore
        #endregion
    }
}
