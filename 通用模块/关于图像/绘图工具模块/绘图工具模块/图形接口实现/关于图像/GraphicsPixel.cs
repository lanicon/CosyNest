using System.DrawingFrancis.Graphics;
using System.Maths;

namespace System.DrawingFrancis
{
    /// <summary>
    /// 这个类型是<see cref="IGraphicsPixel"/>的实现，
    /// 可以被视为一个像素点
    /// </summary>
    class GraphicsPixel : IGraphicsPixel
    {
        #region 获取像素点的颜色
        public IColor Color { get; }
        #endregion
        #region 像素点的位置
        public IPoint Position { get; }
        #endregion
        #region 像素点的图层
        public int Layer { get; }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的颜色，位置和图层初始化像素点
        /// </summary>
        /// <param name="Color">像素点的颜色</param>
        /// <param name="Position">像素点的位置</param>
        /// <param name="Layer">像素点的图层</param>
        public GraphicsPixel(IColor Color, IPoint Position, int Layer = 0)
        {
            this.Color = Color;
            this.Position = Position;
            this.Layer = Layer;
        }
        #endregion
    }
}
