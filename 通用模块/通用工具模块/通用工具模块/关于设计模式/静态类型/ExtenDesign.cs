using System.Collections.Generic;
using System.Design.Direct;

namespace System
{
    /// <summary>
    /// 有关设计模式的扩展方法全部放在这里
    /// </summary>
    public static class ExtenDesign
    {
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
