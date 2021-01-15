using System;
using System.Collections.Generic;
using System.Text;

namespace System.Maths
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个长度单位
    /// </summary>
    public interface IUTLength : IUT
    {
        #region 预设单位
        #region 公制单位
        #region 千米
        /// <summary>
        /// 返回代表千米的单位
        /// </summary>
        public static IUTLength KM { get; }
        = new UTLength("千米", 1000);
        #endregion
        #region 米(公制单位)
        /// <summary>
        /// 返回一个代表米的单位，这是长度单位的公制单位
        /// </summary>
        public static IUTLength MetersMetric { get; }
        = new UTLength("米", 1);
        #endregion
        #region 分米
        /// <summary>
        /// 返回代表分米的单位
        /// </summary>
        public static IUTLength Decimeter { get; }
        = new UTLength("分米", 1.0 / 10);
        #endregion
        #region 厘米
        /// <summary>
        /// 返回代表厘米的单位
        /// </summary>
        public static IUTLength CM { get; }
        = new UTLength("厘米", 1.0 / 100);
        #endregion
        #region 毫米
        /// <summary>
        /// 返回代表毫米的长度单位
        /// </summary>
        public static IUTLength MM { get; }
        = new UTLength("毫米", 1.0 / 1000);
        #endregion
        #endregion
        #region 英制单位
        #region 英寸
        /// <summary>
        /// 返回代表英寸的长度单位
        /// </summary>
        public static IUTLength Inches { get; }
        = new UTLength("英寸", 0.0254);
        #endregion
        #endregion
        #endregion
        #region 创建单位
        #region 使用常数
        /// <summary>
        /// 用指定的名称和换算标准（常数）创建长度单位
        /// </summary>
        /// <param name="Name">长度单位的名称</param>
        /// <param name="Conver">一个用来和公制单位进行换算的常数，
        /// 假设本单位为a，常数为b，公制单位为c，c=a*b，a=c/b</param>
        /// <returns></returns>
        public static IUTLength Create(string Name, Num Conver)
            => new UTLength(Name, Conver);
        #endregion
        #region 使用委托
        /// <summary>
        /// 用指定的名称和转换方法创建长度单位
        /// </summary>
        /// <param name="Name">本单位的名称</param>
        /// <param name="ToMetric">从本单位转换为公制单位的委托</param>
        /// <param name="FromMetric">从公制单位转换为本单位的委托</param>
        /// <returns></returns>
        public static IUTLength Create(string Name, Func<Num, Num> ToMetric, Func<Num, Num> FromMetric)
            => new UTLength(Name, ToMetric, FromMetric);
        #endregion
        #endregion
    }
}
