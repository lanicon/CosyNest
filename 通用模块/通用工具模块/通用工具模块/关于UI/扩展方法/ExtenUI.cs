using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// 关于UI的扩展方法全部放在这里，请注意：
    /// 为了保证平台通用性，仅提供基本API
    /// </summary>
    public static class ExtenUI
    {
        #region 关于INotifyPropertyChanged
        #region 发出属性已修改的通知
        #region 弱事件版本
        /// <summary>
        /// 调用<see cref="INotifyPropertyChanged.PropertyChanged"/>事件，
        /// 自动填入调用属性的名称
        /// </summary>
        /// <param name="Obj">要引发事件的<see cref="INotifyPropertyChanged"/>对象</param>
        /// <param name="Del"><see cref="INotifyPropertyChanged.PropertyChanged"/>事件所在的弱引用封装</param>
        /// <param name="PropertyName">调用属性的名称，可自动获取，如果是<see cref="string.Empty"/>，代表所有属性都已更改</param>
        public static void Changed(this INotifyPropertyChanged Obj, WeakDelegate<PropertyChangedEventHandler>? Del, [CallerMemberName]string? PropertyName = null)
            => Del?.DynamicInvoke(Obj, new PropertyChangedEventArgs(PropertyName));

        /*注释：如果将更改属性名设为String.Empty，
           可以通知索引器已经更改，
           但如果填入索引器的默认名称Item，则不会发出通知，
           原因可能是索引器能够重载*/
        #endregion
        #region 传统事件版本
        /// <summary>
        /// 调用一个<see cref="INotifyPropertyChanged.PropertyChanged"/>事件，
        /// 自动填入调用属性的名称
        /// </summary>
        /// <param name="Obj">要引发事件的<see cref="INotifyPropertyChanged"/>对象</param>
        /// <param name="Del"><see cref="INotifyPropertyChanged.PropertyChanged"/>事件的传统委托封装</param>
        /// <param name="PropertyName">调用属性的名称，可自动获取，如果是<see cref="string.Empty"/>，代表所有属性都已更改</param>
        public static void Changed(this INotifyPropertyChanged Obj, PropertyChangedEventHandler? Del, [CallerMemberName]string? PropertyName = null)
            => Del?.Invoke(Obj, new PropertyChangedEventArgs(PropertyName));
        #endregion
        #endregion
        #endregion
        #region 关于Color
        #region 解构Color
        /// <summary>
        /// 将一个<see cref="Color"/>解构为RGBA
        /// </summary>
        /// <param name="color">待解构的颜色</param>
        /// <param name="R">这个对象接收颜色的R值</param>
        /// <param name="G">这个对象接收颜色的G值</param>
        /// <param name="B">这个对象接收颜色的B值</param>
        /// <param name="A">这个对象接收颜色的A值</param>
        public static void Deconstruct(this Color color, out byte R, out byte G, out byte B, out byte A)
        {
            R = color.R;
            G = color.G;
            B = color.B;
            A = color.A;
        }
        #endregion
        #endregion
    }
}
