using System.Collections.Generic;
using System.Linq;
using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个沉降观测站
    /// </summary>
    public interface ISettlementObservatory : ISettlement
    {
        #region 父代观测点
        /// <summary>
        /// 获取此观测站前视的观测点
        /// </summary>
        new ISettlementPoint Father
            => (ISettlementPoint)Base.Father!;
        #endregion
        #region 子代观测站
        /// <summary>
        /// 枚举此观测站后视的所有观测点
        /// </summary>
        new IEnumerable<ISettlementPoint> Son
            => Base.Son.Cast<ISettlementPoint>();
        #endregion
        #region 添加观测点
        /// <summary>
        /// 添加一个此观测站后视的观测点
        /// </summary>
        /// <param name="Name">观测点的名称</param>
        /// <param name="Recording">后视记录</param>
        /// <returns></returns>
        ISettlementPoint Add(string Name, IUnit<IUTLength> Recording);

        /*实现本API请遵循以下规范：
          #在添加观测点时，应根据名称自动进行判断，
          如果添加的是基准点或者附合点等高程已知的点，应进行特殊处理*/
        #endregion
    }
}
