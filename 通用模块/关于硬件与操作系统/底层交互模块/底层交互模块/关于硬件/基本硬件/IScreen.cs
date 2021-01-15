using System.Maths;

namespace System.Underlying
{
    /// <summary>
    /// 这个接口封装了有关当前硬件屏幕的信息
    /// </summary>
    public interface IScreen
    {
        #region 获取屏幕的分辨率
        /// <summary>
        /// 获取屏幕的分辨率
        /// </summary>
        ISizePixel Resolution { get; }
        #endregion
        #region 获取屏幕的物理大小
        /// <summary>
        /// 获取屏幕的物理大小
        /// </summary>
        (IUnit<IUTLength> Width, IUnit<IUTLength> Height) Size
        {
            get
            {
                var (w, h) = Resolution;
                #region 本地函数
                static IUnit<IUTLength> Get(Num Pixel, int DPI)
                    => CreateBaseMathObj.Unit(Pixel / DPI, IUTLength.Inches);
                #endregion
                return (Get(w, DPIX), Get(h, DPIY));
            }
        }
        #endregion
        #region 获取DPI
        #region X轴DPI
        /// <summary>
        /// 获取当前屏幕X轴的DPI
        /// </summary>
        int DPIX { get; }
        #endregion
        #region Y轴DPI
        /// <summary>
        /// 获取当前屏幕Y轴的DPI
        /// </summary>
        int DPIY { get; }
        #endregion
        #endregion
        #region 获取像素长度
        #region X轴像素
        /// <summary>
        /// 获取X轴像素的长度单位
        /// </summary>
        IUTLength LengthPixelX
            => IUTLength.Create("X轴像素",
            x => IUTLength.Inches.ToMetric(x / DPIX),
            x => IUTLength.Inches.FromMetric(x) * DPIX);
        #endregion
        #region Y轴像素
        /// <summary>
        /// 获取Y轴像素的长度单位
        /// </summary>
        IUTLength LengthPixelY
            => IUTLength.Create("Y轴像素",
            x => IUTLength.Inches.ToMetric(x / DPIY),
            x => IUTLength.Inches.FromMetric(x) * DPIY);
        #endregion
        #endregion
    }
}
