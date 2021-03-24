using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 这个类型代表一个沉降观测点
    /// </summary>
    class SettlementPoint : SettlementPointBase
    {
        #region 高程
        public override IUnit<IUTLength> High
            => Father.To<SettlementBase>()!.High - Recording! - ClosedDifference;
        #endregion
        #region 构造函数
        /// <inheritdoc cref="SettlementPointBase(string)"/>
        /// <param name="Recording">原始记录</param>
        /// <param name="Father">上一站观测站</param>
        public SettlementPoint(string Name, IUnit<IUTLength> Recording, INode Father)
            : base(Name)
        {
            this.Recording = Recording;
            this.Father = Father;
            RefreshClosed();
        }
        #endregion
    }
}
