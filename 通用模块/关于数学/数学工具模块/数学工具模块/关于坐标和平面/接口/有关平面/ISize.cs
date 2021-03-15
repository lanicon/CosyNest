namespace System.Maths
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以用来描述一个二维平面的范围
    /// </summary>
    public interface ISize
    {
        #region 返回平面的宽度和高度
        /// <summary>
        /// 返回一个元组，它的项分别指示平面的宽度和高度
        /// </summary>
        (Num Width, Num Height) Size { get; }

        /*实现本API请遵循以下规范：
          #Width和Height不允许负值，
          如果在构造时传入负值，请抛出异常*/
        #endregion
        #region 扩展平面
        /// <summary>
        /// 将平面进行扩展，并返回扩展后的平面
        /// </summary>
        /// <param name="ExtendedWidth">扩展的宽度，加上原有的宽度以后不能为负值</param>
        /// <param name="ExtendedHeight">扩展的高度，加上原有的高度以后不能为负值</param>
        /// <returns></returns>
        ISize Transform(Num ExtendedWidth, Num ExtendedHeight)
        {
            var (W, H) = this;
            return CreateMathObj.Size(W + ExtendedWidth, H + ExtendedHeight);
        }
        #endregion
        #region 转换为像素平面
        /// <summary>
        /// 将平面转换为像素平面
        /// </summary>
        /// <param name="PixelSize">指定单个像素的大小</param>
        /// <param name="Rounding">当这个平面的宽高不能整除像素的大小时，
        /// 如果这个值为<see langword="true"/>，代表将多余的部分抛弃，
        /// 否则代表将多余的部分补齐为一个像素</param>
        /// <returns></returns>
        ISizePixel ToSizePixel(ISize PixelSize, bool Rounding)
        {
            var (W, H) = this;
            var (PW, PH) = PixelSize;
            return CreateMathObj.SizePixel(
                ToolArithmetic.Sim(W / PW, IsProgressive: !Rounding),
                ToolArithmetic.Sim(H / PH, IsProgressive: !Rounding));
        }
        #endregion
        #region 解构ISize
        /// <summary>
        /// 将这个平面大小解构为宽和高
        /// </summary>
        /// <param name="Width">平面的宽</param>
        /// <param name="Height">平面的高</param>
        void Deconstruct(out Num Width, out Num Height)
        {
            var (W, H) = Size;
            Width = W;
            Height = H;
        }
        #endregion
    }
}
