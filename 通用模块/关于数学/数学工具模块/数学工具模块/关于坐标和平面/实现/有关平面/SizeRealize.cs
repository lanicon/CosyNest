using System;
using System.Collections.Generic;
using System.Text;

namespace System.Maths
{
    /// <summary>
    /// <see cref="ISize"/>的实现，
    /// 可以用来描述一个二维平面的范围
    /// </summary>
    record SizeRealize : ISize
    {
        #region 返回平面的高度和宽度
        public (Num Width, Num Height) Size { get; }
        #endregion
        #region 重写的方法
        #region 重写ToString
        public override string ToString()
        {
            var (W, H) = (ISize)this;
            return $"宽：{W}      高：{H}";
        }
        #endregion 
        #endregion
        #region 构造函数
        /// <summary>
        /// 用指定的宽和高初始化对象
        /// </summary>
        /// <param name="Width">指定的宽</param>
        /// <param name="Height">指定的高</param>
        public SizeRealize(Num Width, Num Height)
        {
            ExceptionIntervalOut.Check((Num)0, null, Width, Height);
            Size = (Width, Height);
        }
        #endregion
    }
}
