namespace System.Maths
{
    /// <summary>
    /// 这个类型是<see cref="ISizePixel"/>的实现，
    /// 可以视为一个像素平面
    /// </summary>
    record SizePixel : ISizePixel
    {
        #region 指示像素数量
        public (int Horizontal, int Vertical) PixelCount { get; }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的横向像素数量和纵向像素数量初始化对象
        /// </summary>
        /// <param name="Horizontal">水平方向的像素数量</param>
        /// <param name="Vertical">垂直方向的像素数量</param>
        public SizePixel(int Horizontal, int Vertical)
        {
            ExceptionIntervalOut.Check(0, null, Horizontal, Vertical);
            PixelCount = (Horizontal, Vertical);
        }
        #endregion
    }
}
