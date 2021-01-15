using System;
using System.Collections.Generic;
using System.Text;

namespace System.Maths
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个角度单位
    /// </summary>
    public interface IUTAngle : IUT
    {
        #region 预设单位
        #region 角度（公制单位）
        /// <summary>
        /// 返回代表角度的单位，这是本单位的公制单位
        /// </summary>
        public static IUTAngle AngleMetric { get; }
        = new UTAngle("角度", x => x, x => x);
        #endregion
        #region 弧度
        #region 弧度常量
        /// <summary>
        /// 这个常量代表1度的角所对应的弧度
        /// </summary>
        private const decimal Corresponding = (decimal)Math.PI / 180;
        #endregion
        #region 正式属性
        /// <summary>
        /// 返回代表弧度的单位
        /// </summary>
        public static IUTAngle Radian { get; }
        = new UTAngle("弧度",
            x => x / Corresponding,
            x => x * Corresponding);
        #endregion
        #endregion
        #endregion
        #region 创建单位
        /// <summary>
        /// 用指定的名称和转换委托创建角度单位
        /// </summary>
        /// <param name="Name">名称</param>
        /// <param name="ToMetric">从本单位转换为公制单位的委托</param>
        /// <param name="FromMetric">从公制单位转换为本单位的委托</param>
        /// <returns></returns>
        public static IUTAngle Create(string Name, Func<Num, Num> ToMetric, Func<Num, Num> FromMetric)
            => new UTAngle(Name, ToMetric, FromMetric);
        #endregion
    }
}
