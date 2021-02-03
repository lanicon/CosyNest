using System.Collections.Generic;
using System.ComponentModel;
using System.Design.Direct;
using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// 有关设计模式的扩展方法全部放在这里
    /// </summary>
    public static class ExtenDesign
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
        public static void Changed(this INotifyPropertyChanged Obj, WeakDelegate<PropertyChangedEventHandler>? Del, [CallerMemberName] string PropertyName = "")
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
        public static void Changed(this INotifyPropertyChanged Obj, PropertyChangedEventHandler? Del, [CallerMemberName] string PropertyName = "")
            => Del?.Invoke(Obj, new PropertyChangedEventArgs(PropertyName));
        #endregion
        #endregion
        #endregion
        #region 关于IDirectView
        #region 将IEnumerable转换为IDirectView
        #region 会检查架构
        /// <summary>
        /// 将<see cref="IEnumerable{T}"/>转换为带架构的数据容器
        /// </summary>
        /// <typeparam name="Direct">数据容器所封装的数据类型</typeparam>
        /// <param name="directs">用来枚举数据的枚举器</param>
        /// <param name="CacheAll">如果这个值为<see langword="true"/>，表示在获取第一个元素时缓存全部元素，
        /// 否则代表逐个缓存元素，正确指定这个参数可以改善性能</param>
        /// <param name="Schema">指定数据容器的架构，
        /// 如果为<see langword="null"/>，则在遍历容器时自动获取</param>
        /// <returns></returns>
        public static IDirectView<Direct> ToDirectView<Direct>(this IEnumerable<Direct> directs, bool CacheAll = true, ISchema? Schema = null)
            where Direct : IDirect
            => new DirectView<Direct>(directs, Schema, true, CacheAll);
        #endregion
        #region 不会检查架构
        /// <summary>
        /// 将<see cref="IEnumerable{T}"/>转换为带架构的数据容器，
        /// 但是它不会检查架构，在确定架构肯定兼容时，这个方法性能更高
        /// </summary>
        /// <typeparam name="Direct">数据容器所封装的数据类型</typeparam>
        /// <param name="directs">用来枚举数据的枚举器</param>
        /// <param name="CacheAll">如果这个值为<see langword="true"/>，表示在获取第一个元素时缓存全部元素，
        /// 否则代表逐个缓存元素，正确指定这个参数可以改善性能</param>
        /// <returns></returns>
        public static IDirectView<Direct> ToDirectViewNotCheck<Direct>(this IEnumerable<Direct> directs, bool CacheAll = true)
            where Direct : IDirect
            => new DirectView<Direct>(directs, null, false, CacheAll);
        #endregion
        #endregion
        #endregion
    }
}
