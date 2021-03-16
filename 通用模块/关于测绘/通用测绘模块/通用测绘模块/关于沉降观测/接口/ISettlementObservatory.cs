using System.Collections.Generic;
using System.Linq;
using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个沉降观测站，它前视观测点
    /// </summary>
    public interface ISettlementObservatory : ISettlementObservation
    {
        #region 返回观测站前视的观测点
        /// <summary>
        /// 返回这个观测站前视的观测点
        /// </summary>
        new ISettlementPoint Father
            => (ISettlementPoint)Base.Father!;
        #endregion
        #region 返回观测站后视的观测点
        /// <summary>
        /// 返回所有这个观测站后视的观测点
        /// </summary>
        new IEnumerable<ISettlementPoint> Son
            => Base.Son.Cast<ISettlementPoint>();
        #endregion
        #region 添加后代观测点
        #region 普通观测点
        /// <summary>
        /// 添加这个观测站后视的观测点
        /// </summary>
        /// <param name="Name">观测点的名称</param>
        /// <param name="Recording">观测点的记录</param>
        /// <returns>被添加的观测点</returns>
        ISettlementPointUnknown Add(string Name, IUnit<IUTLength> Recording);
        #endregion
        #region 附合观测点
        /// <summary>
        /// 添加这个观测站后视的附合观测点
        /// </summary>
        /// <param name="Name">观测点的名称</param>
        /// <param name="Recording">观测点的记录</param>
        /// <param name="High"></param>
        /// <returns></returns>
        ISettlementPoint Add(string Name, IUnit<IUTLength> Recording, IUnit<IUTLength> High);
        #endregion
        #endregion
    }
}
