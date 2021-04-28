namespace System.Collections.Generic
{
    /// <summary>
    /// 表示一个<see cref="IEnumerable{T}"/>对象的缓存
    /// </summary>
    /// <typeparam name="Obj">迭代器的元素类型</typeparam>
    class EnumerableCacheRealize<Obj> : EnumerableCache<Obj>
    {
        #region 说明文档
        /*关于缓存迭代器的建议方法：
          #建议在迭代器调用链的最后手动调用Cache进行缓存，
          而不是在每个返回IEnumerable的方法中都调用Cache进行缓存，正确示例：
          List.Where(x=>x>3).Select(x=>x+1).Cache()
          这样做的好处是避免重复缓存，而且比较清晰，
          因为返回IEnumerable的方法仍然是默认不进行缓存，模式与Net基础库一致*/
        #endregion
        #region 封装的迭代器
        /// <summary>
        /// 获取实际用来迭代元素的迭代器
        /// </summary>
        private IEnumerable<Obj>? PackEnumerable { get; set; }
        #endregion
        #region 返回枚举器
        protected override IEnumerable<Obj> GetEnumeratorRealize()
            => PackEnumerable!;
        #endregion
        #region 清理迭代器
        protected override void Clean()
            => PackEnumerable = null;
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="packEnumerable">实际用来迭代元素的迭代器</param>
        /// <param name="cacheAll">如果这个值为<see langword="true"/>，表示在获取第一个元素时缓存全部元素，
        /// 否则代表逐个缓存元素，正确指定这个参数可以改善性能</param>
        public EnumerableCacheRealize(IEnumerable<Obj> packEnumerable, bool cacheAll)
            : base(cacheAll)
        {
            this.PackEnumerable = packEnumerable;
        }
        #endregion
    }
}
