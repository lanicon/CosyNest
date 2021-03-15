using System.Linq;

namespace System.Collections.Generic.Realize
{
    /// <summary>
    /// 表示一个<see cref="ICollection{Obj}"/>的默认实现
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    public abstract class CollectionRealize<Obj> : ICollection<Obj>, ICollection, IReadOnlyCollection<Obj>
    {
        #region 关于集合本身
        #region 封装的容器
        /// <summary>
        /// 被添加到本集合中的元素，实际上会被添加到这个集合
        /// </summary>
        protected ICollection<Obj> PackCollection { get; }
        #endregion
        #region 返回元素数量
        public virtual int Count
            => PackCollection.Count;
        #endregion
        #region 返回是否只读
        public virtual bool IsReadOnly
            => PackCollection.IsReadOnly;
        #endregion
        #region 返回枚举器
        public virtual IEnumerator<Obj> GetEnumerator()
            => PackCollection.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator()
            => PackCollection.GetEnumerator();
        #endregion
        #endregion
        #region 关于拦截器
        #region 添加元素时的拦截器
        /// <summary>
        /// 在添加元素前，会首先调用这个方法
        /// </summary>
        /// <param name="item">待添加的元素</param>
        protected virtual void AddBefore(Obj item)
        {

        }
        #endregion
        #region 移除元素时的拦截器
        /// <summary>
        /// 在移除一个元素前，执行这个方法
        /// </summary>
        /// <param name="item">待移除的元素</param>
        protected virtual void RemoveBefore(Obj item)
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
        #endregion
        #region 关于元素
        #region 检查元素是否在集合中
        public virtual bool Contains(Obj item)
            => PackCollection.Contains(item);
        #endregion
        #region 对元素的操作
        #region 复制元素（泛型集合）
        public virtual void CopyTo(Obj[] array, int arrayIndex)
            => PackCollection.CopyTo(array, arrayIndex);
        #endregion
        #region 复制元素（非泛型）
        public virtual void CopyTo(Array array, int index)
            => Array.Copy(this.ToArray(), array, index);
        #endregion
        #region 添加元素
        public virtual void Add(Obj item)
        {
            AddBefore(item);
            PackCollection.Add(item);
        }
        #endregion
        #region 删除元素
        public virtual bool Remove(Obj item)
        {
            RemoveBefore(item);
            return PackCollection.Remove(item);
        }
        #endregion
        #region 清空元素
        public virtual void Clear()
        {
            ClearBefore();
            PackCollection.Clear();
        }
        #endregion
        #endregion
        #endregion
        #region 搁置的实现
        /*注释：
          这些成员目前不知道应该如何提供实现，
          因此暂时搁置，如果以后确实需要这些功能，
          再来考虑怎么实现它们*/
        #region 返回是否线程安全
        public virtual bool IsSynchronized
            => throw new NotImplementedException();
        #endregion
        #region 获取用来同步线程的对象
        public virtual object SyncRoot
            => throw new NotImplementedException();
        #endregion
        #endregion
        #region 构造方法
        /// <summary>
        /// 将指定的集合封装到对象中
        /// </summary>
        /// <param name="PackCollection">被封装的集合</param>
        public CollectionRealize(ICollection<Obj> PackCollection)
        {
            this.PackCollection = PackCollection;
        }
        #endregion
    }
}
