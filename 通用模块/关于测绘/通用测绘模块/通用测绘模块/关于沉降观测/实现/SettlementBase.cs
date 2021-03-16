using System.Collections.Generic;
using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 这个抽象类是所有沉降观测的基类
    /// </summary>
    abstract class SettlementBase : ISettlement
    {
        #region 返回对象的接口形式
        /// <summary>
        /// 返回这个对象的接口形式
        /// </summary>
        protected ISettlement Interface => this;
        #endregion
        #region 高程
        public abstract IUnit<IUTLength> High { get; }
        #endregion
        #region 原始高程
        /// <summary>
        /// 返回未经平差的原始高程
        /// </summary>
        protected IUnit<IUTLength> HighOriginal { get; }
        #endregion
        #region 父节点
        public INode? Father { get; set; }
        #endregion
        #region 子节点
        /// <summary>
        /// 返回这个节点的所有子节点
        /// </summary>
        protected LinkedList<SettlementBase> SonField { get; } = new();

        public IEnumerable<INode> Son => SonField;
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="HighOriginal">未经平差的原始高程</param>
        public SettlementBase(IUnit<IUTLength> HighOriginal)
        {
            this.HighOriginal = HighOriginal;
        }
        #endregion
    }
}
