using System.Collections.Generic;

namespace System.Maths
{
    /// <summary>
    /// 这个接口代表一个具有位置的二维平面
    /// </summary>
    public interface ISizePos : ISize
    {
        #region 二维平面的位置
        /// <summary>
        /// 返回二维平面左上角的坐标
        /// </summary>
        IPoint Position { get; }
        #endregion
        #region 返回平面的中心
        /// <summary>
        /// 返回这个平面中心的点坐标
        /// </summary>
        IPoint Center
        {
            get
            {
                var (W, H) = Size;
                return Position.Move(W / 2, H / -2);
            }
        }
        #endregion
        #region 关于顶点
        #region 返回全部四个顶点
        /// <summary>
        /// 返回一个集合，它枚举二维平面的四个顶点
        /// </summary>
        IReadOnlyList<IPoint> Vertex
        {
            get
            {
                var (W, H) = this;
                H = -H;
                return new[]
                {
                    Position,
                    Position.Move(W,0),
                    Position.Move(W,H),
                    Position.Move(0,H)
                };
            }
        }
        #endregion
        #region 返回左上角和右下角的顶点
        /// <summary>
        /// 返回这个平面的界限，
        /// 也就是它左上角和右下角的顶点
        /// </summary>
        (IPoint TopLeft, IPoint BottomRight) Boundaries
        {
            get
            {
                var Vertex = this.Vertex;
                return (Vertex[0], Vertex[2]);
            }
        }
        #endregion
        #endregion
        #region 转换为像素平面
        /// <summary>
        /// 将这个对象转换为带位置的像素平面
        /// </summary>
        /// <param name="PixelSize">指定单个像素的大小</param>
        /// <param name="Rounding">当这个平面的宽高不能整除像素的大小时，
        /// 如果这个值为<see langword="true"/>，代表将多余的部分抛弃，
        /// 否则代表将多余的部分补齐为一个像素</param>
        /// <returns></returns>
        ISizePosPixel ToSizePosPixel(ISize PixelSize, bool Rounding)
        {
            var Size = ToSizePixel(PixelSize, Rounding);
            var (R, T) = Position;
            var (PW, PH) = PixelSize;
            var Point = CreateMathObj.Point(
                ToolArithmetic.Sim(R * PW, isProgressive: !Rounding),
                ToolArithmetic.Sim(T * PH, isProgressive: !Rounding));
            return CreateMathObj.SizePosPixel(Point, Size);
        }
        #endregion
        #region 偏移平面
        /// <summary>
        /// 偏移和扩展平面，并返回偏移后的新平面
        /// </summary>
        /// <param name="ExtendedWidth">扩展平面的宽度，加上原有的宽度以后不能为负值</param>
        /// <param name="ExtendedHeight">扩展平面的高度，加上原有的高度以后不能为负值</param>
        /// <param name="OffsetRight">平面左上角向右偏移的坐标</param>
        /// <param name="OffsetTop">平面左上角向上偏移的坐标</param>
        /// <returns></returns>
        ISizePos Transform(Num ExtendedWidth, Num ExtendedHeight, Num OffsetRight, Num OffsetTop)
            => CreateMathObj.SizePos(Position.Move(OffsetRight, OffsetTop),
                Transform(ExtendedWidth, ExtendedHeight));
        #endregion
        #region 解构ISizePos
        /// <summary>
        /// 将本对象解构为宽，高和位置
        /// </summary>
        /// <param name="Width">宽度</param>
        /// <param name="Height">高度</param>
        /// <param name="Pos">位置</param>
        void Deconstruct(out Num Width, out Num Height, out IPoint Pos)
        {
            var (W, H) = this.Size;
            Width = W;
            Height = H;
            Pos = this.Position;
        }
        #endregion
    }
}
