using System;
using System.Collections.Generic;
using System.Text;
using System.Design;

namespace System.DrawingFrancis
{
    /// <summary>
    /// 这个类型是<see cref="IColor"/>的实现，可以用来表示颜色
    /// </summary>
    class Color : IColor
    {
        #region 红色
        public byte R { get; }
        #endregion
        #region 绿色
        public byte G { get; }
        #endregion
        #region 蓝色值
        public byte B { get; }
        #endregion
        #region 透明度
        public byte A { get; }
        #endregion
        #region 重写的方法
        #region 重写GetHashCode
        public override int GetHashCode()
            => ToolEqual.CreateHash(R, G, B, A);
        #endregion
        #region 重写Equals
        public override bool Equals(object? obj)
            => obj is IColor c &&
            R == c.R &&
            G == c.G &&
            B == c.G &&
            A == c.A;
        #endregion
        #region 重写ToString
        public override string ToString()
        {
            return $"R:{R} G:{G} B:{B}  A:{A}";
        }
        #endregion
        #endregion 
        #region 构造函数
        /// <summary>
        /// 使用指定的红色，绿色，蓝色和透明度初始化对象
        /// </summary>
        /// <param name="R">指定的红色值</param>
        /// <param name="G">指定的绿色值</param>
        /// <param name="B">指定的蓝色值</param>
        /// <param name="A">指定的透明度</param>
        public Color(byte R, byte G, byte B, byte A = 255)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = A;
        }
        #endregion
    }
}
