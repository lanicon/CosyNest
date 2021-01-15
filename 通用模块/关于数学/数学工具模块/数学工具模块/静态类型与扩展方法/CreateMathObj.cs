using System.Maths.Geometric;

namespace System.Maths
{
    /// <summary>
    /// 这个静态类型可以用来帮助创建数学对象
    /// </summary>
    public static class CreateMathObj
    {
        #region 创建坐标
        #region 创建IPoint
        #region 指定水平位置和垂直位置
        /// <summary>
        /// 使用指定的水平位置和垂直位置创建坐标
        /// </summary>
        /// <param name="Right">指定的水平位置，这个值越大代表坐标越靠近右边</param>
        /// <param name="Top">指定的垂直位置，这个值越大代表坐标越靠近顶部</param>
        public static IPoint Point(Num Right, Num Top)
            => new Point(Right, Top);
        #endregion
        #region 指定元组
        /// <summary>
        /// 使用指定的位置创建坐标
        /// </summary>
        /// <param name="Pos">这个元组封装了坐标的水平位置和垂直位置</param>
        public static IPoint Point((Num Right, Num Top) Pos)
            => new Point(Pos.Right, Pos.Top);
        #endregion
        #endregion
        #region 创建IVector
        #region 指定长度和方向
        /// <summary>
        /// 用指定的长度和方向创建向量
        /// </summary>
        /// <param name="Length">向量的长度</param>
        /// <param name="Direction">向量的方向</param>
        public static IVector Vector(Num Length, IUnit<IUTAngle> Direction)
            => new Vector(Length, Direction);
        #endregion
        #region 指定开始和结束位置
        /// <summary>
        /// 使用指定的开始位置和结束位置创建向量
        /// </summary>
        /// <param name="Begin">开始位置</param>
        /// <param name="End">结束位置</param>
        /// <returns></returns>
        public static IVector Vector(IPoint Begin, IPoint End)
            => End.ToPC(Begin);
        #endregion
        #endregion
        #endregion
        #region 创建平面
        #region 创建ISize
        #region 传入宽和高
        /// <summary>
        /// 用指定的宽和高创建<see cref="ISize"/>
        /// </summary>
        /// <param name="Width">指定的宽</param>
        /// <param name="Height">指定的高</param>
        public static ISize Size(Num Width, Num Height)
            => new SizeRealize(Width, Height);
        #endregion
        #region 传入元组
        /// <summary>
        /// 使用指定的宽和高创建<see cref="ISize"/>
        /// </summary>
        /// <param name="Size">这个元组描述二维平面的宽和高</param>
        public static ISize Size((Num Width, Num Height) Size)
            => new SizeRealize(Size.Width, Size.Height);
        #endregion
        #endregion
        #region 创建ISizePos
        #region 传入位置和大小
        /// <summary>
        /// 用指定的位置和大小创建<see cref="ISizePos"/>
        /// </summary>
        /// <param name="pos">指定的位置</param>
        /// <param name="size">指定的大小</param>
        public static ISizePos SizePos(IPoint pos, ISize size)
            => SizePos(pos, size.Size.Width, size.Size.Height);
        #endregion
        #region 传入开始位置和结束位置
        /// <summary>
        /// 使用指定的开始位置和结束位置创建<see cref="ISizePos"/>，
        /// 本方法不要求<paramref name="Begin"/>在<paramref name="End"/>的左上方
        /// </summary>
        /// <param name="Begin">开始位置</param>
        /// <param name="End">结束位置</param>
        /// <returns></returns>
        public static ISizePos SizePos(IPoint Begin, IPoint End)
        {
            var (lx, ly) = Begin;
            var (rx, ry) = End;
            if (lx > rx)
                Tool.Exchange(ref lx, ref rx);
            if (ly < ry)
                Tool.Exchange(ref ly, ref ry);
            return SizePos(Point(lx, ly), rx - lx, ly - ry);
        }
        #endregion
        #region 传入位置，宽度和高度
        /// <summary>
        /// 用指定的位置，宽度和高度创建<see cref="ISizePos"/>
        /// </summary>
        /// <param name="Pos">平面的位置</param>
        /// <param name="Width">平面的宽度</param>
        /// <param name="Height">平面的高度</param>
        public static ISizePos SizePos(IPoint Pos, Num Width, Num Height)
            => new SizePos(Pos, Width, Height);
        #endregion
        #region 传入垂直位置，水平位置，宽度和高度
        /// <summary>
        /// 用指定的垂直位置，水平位置，宽度和高度创建<see cref="ISizePos"/>
        /// </summary>
        /// <param name="Top">指定的垂直位置</param>
        /// <param name="Right">指定的水平位置</param>
        /// <param name="Width">指定的宽度</param>
        /// <param name="Height">指定的宽度</param>
        public static ISizePos SizePos(Num Top, Num Right, Num Width, Num Height)
            => SizePos(new Point(Right, Top), Width, Height);
        #endregion
        #endregion
        #endregion
        #region 创建像素平面
        #region 创建ISizePixel
        #region 传入像素数量
        /// <summary>
        /// 指定水平和垂直方向的像素数量，
        /// 然后创建一个<see cref="ISizePixel"/>
        /// </summary>
        /// <param name="Horizontal">水平方向的像素数量</param>
        /// <param name="Vertical">垂直方向的像素数量</param>
        /// <returns></returns>
        public static ISizePixel SizePixel(int Horizontal, int Vertical)
            => new SizePixel(Horizontal, Vertical);
        #endregion
        #endregion
        #region 创建ISizePosPixel
        #region 传入位置和大小
        /// <summary>
        /// 指定位置和大小，然后创建一个<see cref="ISizePosPixel"/>
        /// </summary>
        /// <param name="FirstPixel">像素平面左上角像素的位置</param>
        /// <param name="Size">像素平面的大小</param>
        /// <returns></returns>
        public static ISizePosPixel SizePosPixel(IPoint FirstPixel, ISizePixel Size)
        {
            var (H, V) = Size;
            return new SizePosPixel(FirstPixel, H, V);
        }
        #endregion
        #region 传入开始位置和结束位置
        /// <summary>
        /// 使用指定的开始位置和结束位置创建<see cref="ISizePosPixel"/>，
        /// 本方法不要求<paramref name="Begin"/>在<paramref name="End"/>的左上方
        /// </summary>
        /// <param name="Begin">开始位置</param>
        /// <param name="End">结束位置</param>
        /// <returns></returns>
        public static ISizePosPixel SizePosPixel(IPoint Begin, IPoint End)
        {
            var (Width, Height, Pos) = SizePos(Begin, End);
            return SizePosPixel(Pos, Width + 1, Height + 1);
        }
        #endregion
        #region 传入位置和像素数量
        /// <summary>
        /// 指定位置，以及水平和垂直方向的像素数量，
        /// 然后创建一个<see cref="ISizePosPixel"/>
        /// </summary>
        /// <param name="FirstPixel">像素平面左上角的像素所处于的位置</param>
        /// <param name="Horizontal">水平方向的像素数量</param>
        /// <param name="Vertical">垂直方向的像素数量</param>
        /// <returns></returns>
        public static ISizePosPixel SizePosPixel(IPoint FirstPixel, int Horizontal, int Vertical)
            => new SizePosPixel(FirstPixel, Horizontal, Vertical);
        #endregion
        #region 传入行，列，水平和垂直位置
        /// <summary>
        /// 指定开始行列数，以及水平和垂直方向的像素数量，
        /// 然后创建一个<see cref="ISizePosPixel"/>
        /// </summary>
        /// <param name="BeginCol">像素平面左上角像素的列数</param>
        /// <param name="BeginRow">像素平面左上角像素的行数</param>
        /// <param name="Horizontal">水平方向的像素数量</param>
        /// <param name="Vertical">垂直方向的像素数量</param>
        /// <returns></returns>
        public static ISizePosPixel SizePosPixel(int BeginCol, int BeginRow, int Horizontal, int Vertical)
            => new SizePosPixel(Point(BeginCol, BeginRow), Horizontal, Vertical);
        #endregion
        #endregion
        #endregion
        #region 创建几何图形和几何模型
        #region 创建线段
        #region 创建模型
        /// <summary>
        /// 创建一个用来创建线段的几何模型
        /// </summary>
        /// <param name="End">线段的终点，起点被固定为(0，0)</param>
        /// <returns></returns>
        public static IGeometricModel<IBessel> ModelLine(IPoint End)
            => new ModelLine(End);
        #endregion
        #region 直接创建线段
        /// <summary>
        /// 直接创建一条线段
        /// </summary>
        /// <param name="Begin">线段的起点，此为绝对坐标</param>
        /// <param name="End">线段的终点，此为绝对坐标</param>
        /// <returns></returns>
        public static IBessel GeometricLine(IPoint Begin, IPoint End)
            => ModelLine(End.ToRel(Begin)).Draw(Begin);
        #endregion
        #endregion
        #region 创建折线
        /// <summary>
        /// 创建一个几何模型，它可以用来创建由一条或多条折线组成的几何图形
        /// </summary>
        /// <param name="close">如果这个值为<see langword="true"/>，则自动闭合这个几何图形</param>
        /// <param name="points">组成几何图形的折线的端点，
        /// 第一个点被固定为(0,0)，不需要输入</param>
        /// <returns></returns>
        public static IGeometricModel<IGeometric> BrokenLine(bool close, params IPoint[] points)
            => new ModelBrokenLine(close, points);
        #endregion
        #endregion
    }
}
