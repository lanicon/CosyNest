using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 这个类型代表一个高程已知的沉降观测点，
    /// 它通常是基准点或附合点
    /// </summary>
    class SettlementPointFixed : SettlementPointBase
    {
        #region 原始高程
        public override IUnit<IUTLength> HighOriginal
            => Father is SettlementObservatory s ?
            s.HighOriginal - Recording! : High;
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
