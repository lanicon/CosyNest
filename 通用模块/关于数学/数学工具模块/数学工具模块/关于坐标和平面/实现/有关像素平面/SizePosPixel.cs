using System;
using System.Collections.Generic;
using System.Text;

namespace System.Maths
{
    /// <summary>
    /// 这个类型是<see cref="ISizePosPixel"/>的实现，
    /// 可以视为一个具有位置的像素平面
    /// </summary>
    record SizePosPixel : SizePixel, ISizePosPixel
    {
        #region 指定第一个像素的位置
        public IPoint FirstPixel { get; }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="FirstPixel">左上角第一个像素的位置</param>
        /// <param name="Horizontal">水平方向像素的数量</param>
        /// <param name="Vertical">垂直方向像素的数量</param>
        public SizePosPixel(IPoint FirstPixel, int Horizontal, int Vertical)
            : base(Horizontal, Vertical)
        {
            var (R, T) = FirstPixel;
            this.FirstPixel = CreateMathObj.Point(R.Rounding(), T.Rounding());
        }
        #endregion
    }
}
