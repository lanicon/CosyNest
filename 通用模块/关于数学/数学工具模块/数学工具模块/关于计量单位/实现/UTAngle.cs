using System;
using System.Collections.Generic;
using System.Text;

namespace System.Maths
{
    /// <summary>
    /// 这个类型是<see cref="IUTAngle"/>的实现，
    /// 可以视为一个角度单位
    /// </summary>
    class UTAngle : UT, IUTAngle
    {
        #region 返回单位的类型
        protected override Type UTType
            => typeof(IUTAngle);
        #endregion
        #region 构造函数
        /// <summary>
        /// 用指定的名称和转换委托初始化角度单位
        /// </summary>
        /// <param name="Name">名称</param>
        /// <param name="ToMetric">从本单位转换为公制单位的委托</param>
        /// <param name="FromMetric">从公制单位转换为本单位的委托</param>
        public UTAngle(string Name, Func<Num, Num> ToMetric, Func<Num, Num> FromMetric)
            : base(Name, ToMetric, FromMetric)
        {

        }
        #endregion
    }
}
