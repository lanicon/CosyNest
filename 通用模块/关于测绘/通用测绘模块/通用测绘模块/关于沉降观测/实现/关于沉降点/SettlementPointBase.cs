using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 这个类型是所有沉降观测点的基类
    /// </summary>
    abstract class SettlementPointBase : SettlementBase, ISettlementPoint
    {
        #region 名称
        public string Name { get; }
        #endregion
        #region 关于闭合与附合
        #region 闭合/附合点
        public ISettlementPoint? Closed { get; }
        #endregion
        #region 闭合/附合差
        public IUnit<IUTLength> ClosedDifference { get; protected set; } = IUnit<IUTLength>.Zero;
        #endregion
        #endregion
        #region 关于添加和移除后代
        #region 添加后代观测站
        public ISettlementObservatory Add(IUnit<IUTLength> Recording)
        {
            throw new Exception();
        }
        #endregion
        #region 移除所有后代
        public void RemoveOffspring()
        {

        }
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Name">沉降观测点的名称</param>
        /// <param name="HighOriginal">未经平差的原始高程</param>
        public SettlementPointBase(string Name, IUnit<IUTLength> HighOriginal)
            : base(HighOriginal)
        {
            this.Name = Name;
        }
        #endregion
    }
}
