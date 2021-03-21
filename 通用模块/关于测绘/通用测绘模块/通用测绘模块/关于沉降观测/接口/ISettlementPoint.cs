using System.Collections.Generic;
using System.Linq;
using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个沉降观测点
    /// </summary>
    public interface ISettlementPoint : ISettlement
    {
        #region 名称
        /// <summary>
        /// 获取沉降观测点的名称
        /// </summary>
        string Name { get; }
        #endregion
        #region 父代观测站
        /// <summary>
        /// 获取后视该观测点的观测站，
        /// 如果为基准点，则为<see langword="null"/>
        /// </summary>
        new ISettlementObservatory? Father
            => (ISettlementObservatory?)Base.Father;
        #endregion
        #region 子代观测站
        /// <summary>
        /// 枚举前视此观测点的所有观测站
        /// </summary>
        new IEnumerable<ISettlementObservatory> Son
            => Base.Son.Cast<ISettlementObservatory>();
        #endregion
        #region 闭合/附合差
        /// <summary>
        /// 获取此观测点的闭合/附合差，
        /// 如果没有闭合/附合，则为0
        /// </summary>
        IUnit<IUTLength> ClosedDifference { get; }
        #endregion
        #region 添加后代观测站
        /// <summary>
        /// 添加一个前视此观测点的观测站
        /// </summary>
        /// <param name="Recording">观测站的前视记录</param>
        /// <returns>新添加的观测站</returns>
        ISettlementObservatory Add(IUnit<IUTLength> Recording);
        #endregion
    }
}
