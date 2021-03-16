
namespace System.Mapping.Settlement
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个高程不是固定，
    /// 而是需要观测的沉降观测点，它后视观测站
    /// </summary>
    public interface ISettlementPointUnknown : ISettlementObservation, ISettlementPoint
    {

    }
}
