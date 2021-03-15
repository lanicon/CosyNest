﻿namespace System.DrawingFrancis
{
    /// <summary>
    /// 凡是实现这个接口的类型，都可以视为一个32位颜色
    /// </summary>
    public interface IColor
    {
        #region 红色
        /// <summary>
        /// 获取红色值
        /// </summary>
        byte R { get; }
        #endregion
        #region 绿色
        /// <summary>
        /// 获取绿色值
        /// </summary>
        byte G { get; }
        #endregion
        #region 蓝色值
        /// <summary>
        /// 获取蓝色值
        /// </summary>
        byte B { get; }
        #endregion
        #region 透明度
        /// <summary>
        /// 获取透明度，
        /// 0表示完全透明，255表示完全不透明
        /// </summary>
        byte A { get; }
        #endregion
        #region 解构Color
        /// <summary>
        /// 将一个<see cref="IColor"/>解构为RGBA
        /// </summary>
        /// <param name="R">这个对象接收颜色的R值</param>
        /// <param name="G">这个对象接收颜色的G值</param>
        /// <param name="B">这个对象接收颜色的B值</param>
        /// <param name="A">这个对象接收颜色的A值</param>
        public void Deconstruct(out byte R, out byte G, out byte B, out byte A)
        {
            R = this.R;
            G = this.G;
            B = this.B;
            A = this.A;
        }
        #endregion
    }
}
