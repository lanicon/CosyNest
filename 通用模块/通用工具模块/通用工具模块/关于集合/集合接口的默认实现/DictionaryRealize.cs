using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic
{
    /// <summary>
    /// 代表一个泛型字典的默认实现
    /// </summary>
    /// <typeparam name="Key">字典的键类型</typeparam>
    /// <typeparam name="Value">字典的值类型</typeparam>
    public class DictionaryRealize<Key, Value> : IDictionary<Key, Value>, IReadOnlyDictionary<Key, Value>
        where Key : notnull
    {
        #region 说明文档
        /*问：为什么ListRealize是抽象类，而本类型不是？
          答：因为本类型实现了IAddOnlyDictionary，如果本类型是抽象类的话，
          要创建IAddOnlyDictionary就需要实现一个新类型*/
        #endregion
        #region 被封装的字典
        /// <summary>
        /// 被封装的字典，这个类型的功能就是通过它实现的
        /// </summary>
        protected IDictionary<Key, Value> PackDictionary { get; }
        #endregion
        #region 关于拦截器
        #region 添加元素时的拦截器
        /// <summary>
        /// 在添加元素前，会先执行这个方法
        /// </summary>
        /// <param name="key">待添加的键</param>
        /// <param name="value">待添加的值</param>
        protected virtual void AddBefore(Key key, Value value)
        {

        }
        #endregion
        #region 移除元素时的拦截器
        /// <summary>
        /// 在移除元素前，会先执行这个方法
        /// </summary>
        /// <param name="key">待移除的键</param>
        protected virtual void RemoveBefore(Key key)
        {

        }
        #endregion
        #region 清空集合时的拦截器
        /// <summary>
        /// 在清空集合前，执行这个方法
        /// </summary>
        protected virtual void ClearBefore()
        {

        }
        #endregion
        #region 访问元素时的拦截器
        /// <summary>
        /// 在访问元素前，会先执行这个方法
        /// </summary>
        /// <param name="key">待访问的键</param>
        /// <param name="value">待访问的值</param>
        protected virtual void AccessBefore(Key key, Value value)
        {

        }
        #endregion
        #region 修改元素时的拦截器
        /// <summary>
        /// 在修改字典中已经存在的元素时，会先执行这个方法
        /// </summary>
        /// <param name="key">被修改的元素的键</param>
        /// <param name="newValue">修改后的新值</param>
        protected virtual void ModifyBefore(Key key, Value newValue)
        {

        }
        #endregion
        #endregion
        #region 关于集合元素
        #region 检查元素是否存在
        #region 检查指定的键
        public virtual bool ContainsKey(Key key)
            => PackDictionary.ContainsKey(key);
        #endregion
        #region 检查指定的键值对
        public bool Contains(KeyValuePair<Key, Value> item)
            => TryGetValue(item.Key, out var value) && Equals(value, item.Value);
        #endregion
        #endregion
        #region 对元素的操作
        #region 添加元素
        #region 传入键和值
        public virtual void Add(Key key, Value value)
        {
            AddBefore(key, value);
            PackDictionary.Add(key, value);
        }
        #endregion
        #region 传入键值对
        public void Add(KeyValuePair<Key, Value> item)
            => Add(item.Key, item.Value);
        #endregion
        #endregion
        #region 移除元素
        #region 移除键
        public virtual bool Remove(Key key)
        {
            RemoveBefore(key);
            return PackDictionary.Remove(key);
        }
        #endregion
        #region 移除键值对
        public bool Remove(KeyValuePair<Key, Value> item)
            => Remove(item.Key);
        #endregion
        #region 移除全部元素
        public virtual void Clear()
        {
            ClearBefore();
            PackDictionary.Clear();
        }
        #endregion
        #endregion
        #region 复制元素
        public virtual void CopyTo(KeyValuePair<Key, Value>[] array, int arrayIndex)
            => PackDictionary.CopyTo(array, arrayIndex);
        #endregion
        #endregion
        #endregion
        #region 关于集合本身
        #region 根据键读写值
        #region 可读可写，会引发异常
        public virtual Value this[Key key]
        {
            get
            {
                var Value = PackDictionary[key];
                AccessBefore(key, Value);
                return Value;
            }
            set
            {
                if (ContainsKey(key))
                    ModifyBefore(key, value);
                else AddBefore(key, value);
                PackDictionary[key] = value;
            }
        }
        #endregion
        #region 只读，不会引发异常
        public virtual bool TryGetValue(Key key, [MaybeNullWhen(false)] out Value value)
        {
            if (PackDictionary.TryGetValue(key, out value))
            {
                AccessBefore(key, value);
                return true;
            }
            return false;
        }
        #endregion
        #endregion
        #region 关于键集合与值集合
        #region 返回键集合
        public virtual ICollection<Key> Keys
            => PackDictionary.Keys;
        #endregion
        #region 返回键集合（只读字典版本）
        IEnumerable<Key> IReadOnlyDictionary<Key, Value>.Keys
            => this.Keys;
        #endregion
        #region 返回值集合
        public virtual ICollection<Value> Values
            => PackDictionary.Values;
        #endregion
        #region 返回值集合（只读字典版本）
        IEnumerable<Value> IReadOnlyDictionary<Key, Value>.Values
            => this.Values;
        #endregion
        #endregion
        #region 返回元素数
        public virtual int Count
            => PackDictionary.Count;
        #endregion
        #region 返回是否只读
        public virtual bool IsReadOnly
            => PackDictionary.IsReadOnly;
        #endregion
        #region 返回枚举器
        public virtual IEnumerator<KeyValuePair<Key, Value>> GetEnumerator()
            => PackDictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
        #endregion
        #endregion
        #region 构造方法
        /// <summary>
        /// 将指定的字典封装进对象
        /// </summary>
        /// <param name="PackDictionary">被封装的字典，
        /// 如果为<see langword="null"/>，则自动初始化一个D<see cref="Dictionary{TKey, TValue}"/>填入</param>
        public DictionaryRealize(IDictionary<Key, Value>? PackDictionary = null)
        {
            this.PackDictionary = PackDictionary ?? new Dictionary<Key, Value>();
        }
        #endregion
    }
}
