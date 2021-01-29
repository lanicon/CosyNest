using System.DrawingFrancis;

namespace System
{
    /// <summary>
    /// 关于绘图的扩展方法全部放在这里
    /// </summary>
    public static class ExtenDrawing
    {
        #region 将IColor转换为Color
        /// <summary>
        /// 将<see cref="IColor"/>转换为等价的微软原生<see cref="Drawing.Color"/>
        /// </summary>
        /// <param name="C"></param>
        /// <returns></returns>
        public static Drawing.Color ToColor(this IColor C)
            => Drawing.Color.FromArgb(C.A, C.R, C.G, C.B);
        #endregion
        #region 将Color转换为IColor
        /// <summary>
        /// 将微软原生的<see cref="Drawing.Color"/>转换为等价的<see cref="IColor"/>
        /// </summary>
        /// <param name="C"></param>
        /// <returns></returns>
        public static IColor ToColor(this Drawing.Color C)
        {
            var (r, g, b, a) = C;
            return CreateDrawingObj.Color(r, g, b, a);
        }
        #endregion
        #region 解构Color
        /// <summary>
        /// 将一个<see cref="Color"/>解构为RGBA
        /// </summary>
        /// <param name="color">待解构的颜色</param>
        /// <param name="R">这个对象接收颜色的R值</param>
        /// <param name="G">这个对象接收颜色的G值</param>
        /// <param name="B">这个对象接收颜色的B值</param>
        /// <param name="A">这个对象接收颜色的A值</param>
        public static void Deconstruct(this Drawing.Color color, out byte R, out byte G, out byte B, out byte A)
        {
            R = color.R;
            G = color.G;
            B = color.B;
            A = color.A;
        }
        #endregion
    }
}
