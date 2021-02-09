﻿using System.Maths;
using System.Runtime.InteropServices;

namespace System.Underlying.PC
{
    /// <summary>
    /// 这个类型是<see cref="IScreen"/>的实现，
    /// 封装了当前PC硬件屏幕的信息
    /// </summary>
    class ScreenPC : IScreen
    {
        #region 获取屏幕的分辨率
        public ISizePixel Resolution { get; }
        #endregion
        #region 获取DPI
        #region X轴DPI
        public int DPIX { get; }
        #endregion
        #region Y轴DPI
        public int DPIY { get; }
        #endregion 
        #endregion
        #region 构造函数
        /// <summary>
        /// 无参数构造函数
        /// </summary>
        public ScreenPC()
        {
            DPIX = DpiX();
            DPIY = DpiY();
            Resolution = DESKTOP();
        }
        #endregion
        #region Win32API调用
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        const int LOGPIXELSX = 88;

        const int LOGPIXELSY = 90;

        const int DESKTOPVERTRES = 117;

        const int DESKTOPHORZRES = 118;

        private static int DpiX()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            int DpiX = GetDeviceCaps(hdc, LOGPIXELSX);
            ReleaseDC(IntPtr.Zero, hdc); return DpiX;
        }

        private static int DpiY()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            int DpiX = GetDeviceCaps(hdc, LOGPIXELSY);
            ReleaseDC(IntPtr.Zero, hdc); return DpiX;
        }

        /// <summary>
        /// 获取真实设置的桌面分辨率大小
        /// </summary>
        private static ISizePixel DESKTOP()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            var size = CreateMathObj.SizePixel(
                GetDeviceCaps(hdc, DESKTOPHORZRES),
                GetDeviceCaps(hdc, DESKTOPVERTRES));
            ReleaseDC(IntPtr.Zero, hdc);
            return size;
        }
        #endregion 
    }
}
