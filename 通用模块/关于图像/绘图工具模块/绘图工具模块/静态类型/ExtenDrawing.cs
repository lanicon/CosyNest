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
    }
}
