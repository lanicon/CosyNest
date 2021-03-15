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
    }
}
