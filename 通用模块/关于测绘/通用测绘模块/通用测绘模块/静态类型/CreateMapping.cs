using System.Mapping.Settlement;
using System.Maths;

namespace System.Mapping
{
    /// <summary>
    /// 这个静态工具类可以用来帮助创建和测绘有关的对象
    /// </summary>
    public static class CreateMapping
    {
        #region 返回沉降观测专用长度单位
        /// <summary>
        /// 返回沉降观测专用长度单位，
        /// 它等于百分之一毫米
        /// </summary>
        /// <returns></returns>
        public static IUTLength UTSettlement { get; }
            = IUTLength.Create("沉降观测长度单位", 0.00001);
        #endregion
        #region 创建沉降观测站
        #region 使用已知高程
        /// <summary>
        /// 使用已知高程创建沉降观测站，
        /// 它一般是基准点或附合点
        /// </summary>
        /// <param name="Name">沉降观测点的名称</param>
        /// <param name="High">沉降观测点的高程</param>
        /// <returns></returns>
        public static ISettlement Settlement(string Name, IUTLength High)
            => new Settlement.Settlement(Name) { High = High };
        #endregion
        #endregion
    }
}
