
using System.Collections.Generic;
using System.Linq;
using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个沉降观测点，
    /// 它的高程可能是已知的，也可能是未知的
    /// </summary>
    public interface ISettlementPoint : ISettlement
    {
        #region 关于前站和后站
        #region 返回观测点后视的观测站
        /// <summary>
        /// 返回这个观测点后视的观测站，
        /// 如果该观测点是基准点，则返回<see langword="null"/>
        /// </summary>
        new ISettlementObservatory? Father
            => (ISettlementObservatory?)Base.Father;
        #endregion
        #region 返回前视这个观测点的观测站
        /// <summary>
        /// 返回所有前视这个观测点的观测站
        /// </summary>
        new IEnumerable<ISettlementObservatory> Son
            => Base.Son.Cast<ISettlementObservatory>();
        #endregion
        #endregion
        #region 关于附合与闭合
        #region 闭合/附合点
        /// <summary>
        /// 获取与这个观测点闭合或附合的点，
        /// 如果没有，返回<see langword="null"/>
        /// </summary>
        ISettlementPoint? Closed { get; }
        #endregion
        #region 闭合/附合差
        /// <summary>
        /// 获取这个观测点的闭合或附合差，
        /// 如果没有闭合或者附合，则为0
        /// </summary>
        ISettlementPoint? ClosedDifference { get; }
        #endregion
        #endregion 
        #region 名称
        /// <summary>
        /// 获取沉降观测点的名称
        /// </summary>
        string Name { get; }
        #endregion
        #region 关于添加和移除后代
        #region 移除所有后代观测站
        /// <summary>
        /// 移除这个观测点的所有后代观测站
        /// </summary>
        void RemoveOffspring();
        #endregion
        #region 添加后代观测站
        /// <summary>
        /// 添加前视这个观测点的观测站
        /// </summary>
        /// <param name="Recording">前视记录</param>
        /// <returns>被添加的观测站</returns>
        ISettlementObservatory Add(IUnit<IUTLength> Recording);
        #endregion
        #endregion 
    }
}
