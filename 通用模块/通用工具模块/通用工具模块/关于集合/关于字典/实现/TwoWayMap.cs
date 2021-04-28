using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// 这个类型是<see cref="ITwoWayMap{A, B}"/>的实现，
    /// 可以作为一个双向映射表
    /// </summary>
    /// <typeparam name="A">要映射的第一个对象类型</typeparam>
    /// <typeparam name="B">要映射的第二个对象类型</typeparam>
    class TwoWayMap<A, B> : ITwoWayMap<A, B>
       where A : notnull
       where B : notnull
    {
        #region 封装的映射表
        #region 从A映射到B
        /// <summary>
        /// 这个表将对象A映射到对象B
        /// </summary>
        private IDictionary<A, B> AToB { get; }
        = new Dictionary<A, B>();
        #endregion
        #region 从B映射到A
        /// <summary>
        /// 这个表将对象B映射到对象A
        /// </summary>
        private IDictionary<B, A> BToA { get; }
        = new Dictionary<B, A>();
        #endregion
        #endregion
        #region 获取映射的值
        #region 将A映射为B
        #region 会引发异常
        public B this[A a]
              => AToB[a];
        #endregion
        #region 不会引发异常
        public (bool Exist, B? Value) TryGetValue(A Key, LazyPro<B>? NoFound = null)
             => AToB.TryGetValue(Key, NoFound);
        #endregion
        #endregion
        #region 将B映射为A
        #region 会引发异常
        public A this[B b]
              => BToA[b];
        #endregion
        #region 不会引发异常
        public (bool Exist, A? Value) TryGetValue(B Key, LazyPro<A>? NoFound = null)
             => BToA.TryGetValue(Key, NoFound);
        #endregion
        #endregion
        #endregion
        #region 注册映射
        #region 注册双向映射
        public void RegisteredTwo(params (A a, B b)[] Map)
        {
            foreach ((var a, var b) in Map)
            {
                AToB.Add(a, b);
                BToA.Add(b, a);
            }
        }
        #endregion
        #region 注册从A到B的单向映射
        public void RegisteredOne(B To, params A[] From)
            => From.ForEach(x => AToB.Add(x, To));
        #endregion
        #region 注册从B到A的单向映射
        public void RegisteredOne(A To, params B[] From)
               => From.ForEach(x => BToA.Add(x, To));
        #endregion
        #endregion
        #region 构造函数
        #region 使用指定的双向映射
        /// <summary>
        /// 构造双向映射表，
        /// 并将指定的双向映射添加到表中
        /// </summary>
        /// <param name="Map">这些元组的项会互相映射</param>
        public TwoWayMap(IEnumerable<(A a, B b)> Map)
        {
            RegisteredTwo(Map.ToArray());
        }
        #endregion
        #region 无参数构造函数
        /// <summary>
        /// 不使用参数构造对象
        /// </summary>
        public TwoWayMap()
        {

        }
        #endregion
        #endregion
    }
}
