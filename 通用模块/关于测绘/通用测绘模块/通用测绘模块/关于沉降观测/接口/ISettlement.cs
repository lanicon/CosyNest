using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 这个接口是所有沉降观测的基接口
    /// </summary>
    public interface ISettlement : INode<ISettlement>
    {
        #region 返回父接口形式
        /// <summary>
        /// 返回本接口的父接口形式
        /// </summary>
        private protected INode Base => this;
        #endregion
        #region 返回基准点
        /// <summary>
        /// 返回沉降观测的基准点
        /// </summary>
        new ISettlementPoint Ancestors
            => (ISettlementPoint)Base.Ancestors;
        #endregion
        #region 高程
        /// <summary>
        /// 获取沉降观测的高程
        /// </summary>
        IUnit<IUTLength> High { get; }
        #endregion
    }
}
