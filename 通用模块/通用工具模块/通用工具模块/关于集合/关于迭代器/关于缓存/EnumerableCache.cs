using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// 凡本类型的派生类均具有迭代器缓存功能，
    /// 它仍然会延迟迭代元素，但是在遍历过一次以后会缓存迭代结果
    /// </summary>
    /// <typeparam name="Obj"></typeparam>
    public abstract class EnumerableCache<Obj> : IEnumerable<Obj>
    {
        #region 迭代器的缓存
        /// <summary>
        /// 获取或设置迭代器的缓存
        /// </summary>
        [DisallowNull]
        private IEnumerable<Obj>? Cache { get; set; }
        #endregion
        #region 指定缓存模式
        /// <summary>
        /// 如果这个值为<see langword="true"/>，表示在获取第一个元素时缓存全部元素，
        /// 否则代表逐个缓存元素，正确指定这个属性可以改善性能
        /// </summary>
        private bool CacheAll { get; }
        #endregion
        #region 获取迭代器
        #region 正式方法
        public IEnumerator<Obj> GetEnumerator()
        {
            var enumerable = Cache ?? GetEnumeratorRealize();
            if (enumerable is
                ICollection<Obj> or
                Array or
                IReadOnlyCollection<Obj> or
                EnumerableCache<Obj>)           //如果迭代器不是延迟返回的，则无需缓存
            {
                return enumerable.GetEnumerator();
            }
            return (CacheAll ? CacheAllMode(enumerable) : CacheOneByOneMode(enumerable)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
        #endregion
        #region 用于枚举元素的模板方法
        /// <summary>
        /// 用于枚举元素的模板方法，
        /// 本类型的子元素实际从这里获取
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerable<Obj> GetEnumeratorRealize();
        #endregion
        #region 缓存全部元素的枚举器
        /// <summary>
        /// 获取遍历第一个元素时缓存全部元素的迭代器，
        /// 它在<see cref="CacheAll"/>为<see langword="true"/>时工作
        /// </summary>
        /// <param name="collections">待缓存的迭代器</param>
        /// <returns></returns>
        private IEnumerable<Obj> CacheAllMode(IEnumerable<Obj> collections)
        {
            Cache = collections.ToArray();
            Clean();
            return Cache;
        }
        #endregion
        #region 逐个缓存元素的迭代器
        /// <summary>
        /// 获取逐个缓存元素的迭代器，
        /// 它在<see cref="CacheAll"/>为<see langword="false"/>时工作
        /// </summary>
        /// <param name="collections">待缓存的迭代器</param>
        /// <returns></returns>
        private IEnumerable<Obj> CacheOneByOneMode(IEnumerable<Obj> collections)
        {
            var cache = new LinkedList<Obj>();
            foreach (var item in collections)
            {
                yield return item;
                cache.AddLast(item);
            }
            Cache = cache;
            Clean();
        }
        #endregion
        #endregion
        #region 清理迭代器
        /// <summary>
        /// 由于<see cref="GetEnumeratorRealize"/>只需要被调用一次，
        /// 因此在第一次迭代集合后，会调用这个方法清理不需要的对象
        /// </summary>
        protected abstract void Clean();
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的缓存模式初始化对象
        /// </summary>
        /// <param name="cacheAll">如果这个值为<see langword="true"/>，表示在获取第一个元素时缓存全部元素，
        /// 否则代表逐个缓存元素，正确指定这个参数可以改善性能</param>
        public EnumerableCache(bool cacheAll)
        {
            this.CacheAll = cacheAll;
        }
        #endregion
    }
}
