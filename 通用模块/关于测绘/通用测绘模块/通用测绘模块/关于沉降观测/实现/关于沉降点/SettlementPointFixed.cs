using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 这个类型代表一个高程已知的沉降观测点，
    /// 它通常是基准点或附合点
    /// </summary>
    class SettlementPointFixed : SettlementPointBase
    {
        #region 是否为已知点
        public override bool IsKnown => true;
        #endregion
        #region 计算得出的高程
        /// <summary>
        /// 获取经计算得出的高程，
        /// 它跟已知的固定高程是不一致的
        /// </summary>
        public IUnit<IUTLength> HighCalculation
            => Father is SettlementBase s ? s.High - Recording! : High;
        #endregion
        #region 高程
        public override IUnit<IUTLength> High { get; }
        #endregion
        #region 构造函数
        /// <inheritdoc cref="SettlementPointBase(string)"/>
        /// <param name="Recording">原始记录</param>
        /// <param name="High">基准点的高程</param>
        /// <param name="Father">指定上一站</param>
        public SettlementPointFixed(string Name, IUnit<IUTLength>? Recording, IUnit<IUTLength> High, INode? Father)
            : base(Name)
        {
            this.High = High;
            this.Recording = Recording;
            this.Father = Father;
            if (Father is { })
                RefreshClosed();
        }
        #endregion
    }
}
