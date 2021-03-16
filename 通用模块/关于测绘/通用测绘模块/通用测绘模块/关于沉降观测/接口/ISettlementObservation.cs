using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个沉降观测中的一站，
    /// 它可能是观测站前视观测点，
    /// 也可能是观测点后视观测站
    /// </summary>
    public interface ISettlementObservation : ISettlement
    {
        #region 记录
        /// <summary>
        /// 获取沉降观测的记录
        /// </summary>
        IUnit<IUTLength> Recording { get; }
        #endregion
    }
}
