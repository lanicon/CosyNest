namespace System.Collections.Generic
{
    /// <summary>
    /// 这个类型代表一个只能添加元素，不能删除元素的字典
    /// </summary>
    /// <typeparam name="Key">字典的键类型</typeparam>
    /// <typeparam name="Value">字典的值类型</typeparam>
    class AddOnlyDictionary<Key, Value> : DictionaryRealize<Key, Value>, IAddOnlyDictionary<Key, Value>
        where Key : notnull
    {
        #region 指示是否可修改元素
        public bool CanModify { get; }
        #endregion
        #region 重写修改元素拦截器
        protected override void ModifyBefore(Key key, Value newValue)
        {
            if (!CanModify)
                throw new NotSupportedException("该字典只能添加元素，不能修改已有的元素");
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="PackDictionary">被封装的字典，
        /// 如果为<see langword="null"/>，则自动初始化一个D<see cref="Dictionary{TKey, TValue}"/>填入</param>
        /// <param name="CanModify">如果这个值为<see langword="true"/>，
        /// 代表可以修改已经被添加到字典的值，否则代表禁止修改</param>
        public AddOnlyDictionary(IDictionary<Key, Value>? PackDictionary = null, bool CanModify = false)
            : base(PackDictionary)
        {
            this.CanModify = CanModify;
        }
        #endregion
    }
}
