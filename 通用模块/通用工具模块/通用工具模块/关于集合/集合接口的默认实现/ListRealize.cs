namespace System.Collections.Generic.Realize
{
    /// <summary>
    /// 表示一个<see cref="IList{T}"/>的默认实现
    /// </summary>
    /// <typeparam name="Obj">集合元素的类型</typeparam>
    public abstract class ListRealize<Obj> : CollectionRealize<Obj>, IList<Obj>, IList, IReadOnlyList<Obj>
    {
        #region 说明文档
        /*注意事项：
          出于最小耦合考虑，父类的PackContainer属性只需要实现ICollection，
          但是为了使本类型正常工作，本类型的这个属性需要实现IList*/
        #endregion
        #region 封装的容器
        #region 封装的IList容器
        /// <summary>
        /// 返回一个IList容器，
        /// 这是被添加进集合的元素实际所处的地方
        /// </summary>
        protected new IList<Obj> PackCollection
            => (IList<Obj>)base.PackCollection;
        #endregion
        #endregion
        #region 访问元素时的拦截器
        /// <summary>
        /// 在访问元素前，会执行这个方法
        /// </summary>
        /// <param name="index">待访问的元素索引</param>
        /// <param name="item">待访问的元素</param>
        protected virtual void AccessBefore(int index, Obj item)
        {

        }
        #endregion
        #region 关于元素
        #region 按索引访问
        public virtual Obj this[int index]
        {
            get
            {
                var value = PackCollection[index];
                AccessBefore(index, value);
                return value;
            }
            set
            {
                AddBefore(value);
                RemoveBefore(this[index]);
                PackCollection[index] = value;
            }
        }
        #endregion
        #region 返回元素的索引
        public virtual int IndexOf(Obj item)
            => PackCollection.IndexOf(item);
        #endregion
        #region 对元素的操作
        #region 插入元素
        public virtual void Insert(int index, Obj item)
        {
            AddBefore(item);
            PackCollection.Insert(index, item);
        }
        #endregion
        #region 按索引移除元素
        public virtual void RemoveAt(int index)
        {
            RemoveBefore(this[index]);
            PackCollection.RemoveAt(index);
        }
        #endregion
        #endregion
        #endregion
        #region 非泛型IList的实现
        /*我不喜欢纯粹为了兼容而存在的非泛型集合，
         因此把它们统一扔在这里*/
        int IList.Add(object? value)
        {
            if (value is Obj a)
            {
                Add(a);
                return Count;
            }
            return -1;
        }
        bool IList.Contains(object? value)
            => value is Obj a && Contains(a);
        int IList.IndexOf(object? value)
            => value is Obj a ? IndexOf(a) : -1;
        void IList.Insert(int index, object? value)
        {
            if (value is Obj a)
                Insert(index, a);
        }
        void IList.Remove(object? value)
        {
            if (value is Obj a)
                Remove(a);
        }
        object? IList.this[int index]
        {
            get => this.To<IList<Obj>>()[index];
            set
            {
                if (value is Obj a)
                    this.To<IList<Obj>>()[index] = a;
            }
        }
        public bool IsFixedSize
            => false;
        #endregion
        #region 构造方法
        /// <summary>
        /// 将指定的IList集合封装进对象中
        /// </summary>
        /// <param name="PackCollection">被封装的集合，
        /// 如果为<see langword="null"/>，则自动初始化一个<see cref="List{T}"/></param>
        public ListRealize(IList<Obj>? PackCollection = null)
            : base(PackCollection ?? new List<Obj>())
        {

        }
        #endregion
    }
}
