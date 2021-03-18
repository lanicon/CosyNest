using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 这个类型代表一个沉降观测点
    /// </summary>
    class SettlementPoint : SettlementPointBase
    {
        #region 高程
        public override IUnit<IUTLength> High => throw new NotImplementedException();
        #endregion
        #region 构造函数
        /// <inheritdoc cref="SettlementPointBase(string, IUnit{IUTLength})"/>
        /// <param name="Father">上一站观测点的名称</param>
        public SettlementPoint(string Name, IUnit<IUTLength> HighOriginal, INode? Father)
            : base(Name, HighOriginal)
        {
            this.Father = Father;
        }
        #endregion
    }
}
