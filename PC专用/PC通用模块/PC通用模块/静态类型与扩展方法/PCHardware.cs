using System;
using System.Collections.Generic;
using System.Text;

namespace System.Underlying.PC
{
    /// <summary>
    /// 这个静态类可以返回当前PC上有关硬件的接口对象
    /// </summary>
    public static class PCHardware
    {
        #region 关于IScreen
        #region 获取默认屏幕
        /// <summary>
        /// 获取默认的<see cref="IScreen"/>对象
        /// </summary>
        public static IScreen ScreenDefault
            => ScreenPC.Only;
        #endregion
        #region 获取或设置当前屏幕
        private static IScreen? ScreenField;
        /// <summary>
        /// 获取或设置当前屏幕，
        /// 此属性禁止写入<see langword="null"/>值
        /// </summary>
        public static IScreen Screen
        {
            get => ScreenField ??= ScreenDefault;
            set => ScreenField = value ?? throw new ArgumentNullException($"{nameof(Screen)}禁止写入null值");
        }
        #endregion
        #endregion
        #region 关于IPower
        #region 获取默认电源
        /// <summary>
        /// 获取默认的<see cref="IPower"/>对象
        /// </summary>
        public static IPower PowerDefault
            => PowerPC.Only;
        #endregion
        #region 获取或设置当前电源
        private static IPower? PowerField;
        /// <summary>
        /// 获取或设置当前电源，
        /// 此属性禁止写入<see langword="null"/>值
        /// </summary>
        public static IPower Power
        {
            get => PowerField ??= PowerDefault;
            set => PowerField = value ?? throw new ArgumentNullException($"{nameof(Power)}禁止写入null值");
        }
        #endregion
        #endregion
        #region 关于IPrinterPanel
        #region 获取默认打印面板
        /// <summary>
        /// 获取默认的<see cref="IPrinterPanel"/>对象
        /// </summary>
        public static IPrinterPanel PrinterPanelDefault
            => PrinterPanelPC.Only;
        #endregion
        #region 获取或设置当前打印面板
        private static IPrinterPanel? PrinterPanelField;
        /// <summary>
        /// 获取或设置当前打印面板，
        /// 这个属性禁止写入<see langword="null"/>值
        /// </summary>
        public static IPrinterPanel PrinterPanel
        {
            get => PrinterPanelField ??= PrinterPanelDefault;
            set => PrinterPanelField = value ?? throw new ArgumentNullException($"{nameof(PrinterPanel)}禁止写入null值");
        }
        #endregion
        #endregion
    }
}
