using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为沉降观测中的一站，
    /// 它的高程可能是已知的，也有可能需要通过记录计算出来
    /// </summary>
    public interface ISettlement : INode<ISettlement>
    {
        #region 名称
        /// <summary>
        /// 获取所观测的沉降点的名称
        /// </summary>
        string Name { get; }
        #endregion
        #region 高程
        /// <summary>
        /// 获取这一站的高程
        /// </summary>
        IUTLength High { get; }
        #endregion
        #region 闭合站
        /// <summary>
        /// 获取这一站的闭合站（如果有）
        /// </summary>
        ISettlement? Closed { get; }
        #endregion
        #region 闭合差
        /// <summary>
        /// 获取这一站的闭合差（如果有）
        /// </summary>
        IUTLength? ClosedDifference { get; }
        #endregion
        #region 添加下一站
        /// <summary>
        /// 以这一站为前视，添加下一站观测
        /// </summary>
        /// <param name="Name">下一站的名称，
        /// 如果该名称在前面观测的点中已经存在，则自动闭合</param>
        /// <param name="Recording">下一站的记录</param>
        /// <returns>被添加的下一站沉降观测对象</returns>
        ISettlementObservation Add(string Name, IUTLength Recording);
        #endregion
    }
}
