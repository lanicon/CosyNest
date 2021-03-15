using System.Collections.Generic;

namespace System.Maths
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个具有位置的像素平面
    /// </summary>
    public interface ISizePosPixel : ISizePixel
    {
        #region 说明文档
        /*实现本接口请遵循以下规范：
          #本接口的所有API所返回的IPoint对象不是坐标，
          而是通过像素来表示位置，例如(1,1)表示该像素位于第2行第2列，
          而不是X和Y坐标都为1

          #基于上一条，虽然IPoint使用有理数类型Num来表示，
          但是本接口所有API所返回的IPoint对象都只能出现整数，
          这是因为像素是原子化不可分割的*/
        #endregion
        #region 第一个像素的位置
        /// <summary>
        /// 返回左上角第一个像素的位置
        /// </summary>
        IPoint FirstPixel { get; }
        #endregion
        #region 关于顶点
        #region 返回全部四个顶点
        /// <summary>
        /// 返回一个集合，它枚举像素平面的四个顶点的像素位置
        /// </summary>
        IReadOnlyList<IPoint> Vertex
        {
            get
            {
                var (H, V) = this;
                H = ToolArithmetic.Limit(true, H - 1, 0);
                V = -ToolArithmetic.Limit(true, V - 1, 0);
                return new[]
                {
                    FirstPixel,
                    FirstPixel.Move(H,0),
                    FirstPixel.Move(H,V),
                    FirstPixel.Move(0,V)
                };
            }
        }
        #endregion
        #region 返回左上角和右下角的顶点
        /// <summary>
        /// 返回这个像素平面的界限，
        /// 也就是它左上角和右下角的像素位置
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
        #region 将像素平面转换为坐标平面
        /// <summary>
        /// 将像素平面转换为具有位置的坐标平面
        /// </summary>
        /// <param name="PixelSize">每个像素的大小，
        /// 如果为<see langword="null"/>，则默认为(1,1)</param>
        /// <returns></returns>
        ISizePos ToSizePos(ISize? PixelSize = null)
        {
            var (Width, Height) = PixelSize ??= CreateMathObj.Size(1, 1);
            var (R, T) = FirstPixel;
            var Size = ToSize(PixelSize);
            return CreateMathObj.SizePos(
                CreateMathObj.Point(Width * (R - 1), Height * (T - 1)), Size);
        }
        #endregion
        #region 偏移像素平面
        /// <summary>
        /// 偏移和扩展像素平面，并返回偏移后的新像素平面
        /// </summary>
        /// <param name="ExtendedHorizontal">水平方向扩展的像素数量，
        /// 在加上原有的像素数量后不能为负值</param>
        /// <param name="ExtendedVertical">垂直方向扩展的像素数量，
        /// 在加上原有的像素数量后不能为负值</param>
        /// <param name="OffsetRight">平面左上角向右偏移的像素数量</param>
        /// <param name="OffsetTop">平面左上角向上偏移的像素数量</param>
        /// <returns></returns>
        ISizePosPixel Transform(int ExtendedHorizontal, int ExtendedVertical, int OffsetRight, int OffsetTop)
            => CreateMathObj.SizePosPixel(
                FirstPixel.Move(OffsetRight, OffsetTop),
                Transform(ExtendedHorizontal, ExtendedVertical));
        #endregion
        #region 解构ISizePosPixel
        /// <summary>
        /// 解构本对象
        /// </summary>
        /// <param name="Horizontal">用来接收水平方向像素点数量的对象</param>
        /// <param name="Vertical">用来接收垂直方向像素点数量的对象</param>
        /// <param name="FirstPixel">用来接收左上角第一个像素的位置的对象</param>
        void Deconstruct(out int Horizontal, out int Vertical, out IPoint FirstPixel)
        {
            var (H, V) = this;
            Horizontal = H;
            Vertical = V;
            FirstPixel = this.FirstPixel;
        }
        #endregion
    }
}
