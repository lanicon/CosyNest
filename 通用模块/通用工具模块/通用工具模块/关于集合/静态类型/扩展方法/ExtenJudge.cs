using System.Collections.Generic;

namespace System.Linq
{
    public static partial class ExtenIEnumerable
    {
        //所有关于集合的判断与筛选的方法，全部放在这个类型中
        #region 有关包含关系
        #region 返回一个集合与另一个集合的包含关系
        /// <summary>
        /// 判断集合A相对于集合B的包含关系
        /// </summary>
        /// <typeparam name="Obj">要比较的集合的元素类型</typeparam>
        /// <param name="ListA">集合A</param>
        /// <param name="ListB">集合B</param>
        /// <param name="IsRelational">如果这个值为<see langword="true"/>，
        /// 表示判断基于关系模型，不会考虑重复元素和元素的顺序</param>
        /// <returns></returns>
        public static CollectionContains IsSupersetOf<Obj>(this IEnumerable<Obj> ListA, IEnumerable<Obj> ListB, bool IsRelational = false)
        {
            #region 遵循关系模型
            CollectionContains FollowRelational()
            {
                var A = ListA.ToHashSet();
                var B = ListB.ToHashSet();
                if (A.Count > B.Count)
                    return B.All(x => A.Contains(x)) ? CollectionContains.Superset : CollectionContains.NotMatter;
                return A.All(x => B.Contains(x)) ?
                    (A.Count < B.Count ? CollectionContains.Subset : CollectionContains.Equal) :
                    CollectionContains.NotMatter;
            }
            #endregion
            #region 不遵循关系模型
            CollectionContains NotRelational()
            {
                var (A, B) = (ListA.GetEnumerator(), ListB.GetEnumerator());
                CollectionContains Get(bool AHas, bool BHas)
                    => (AHas, BHas) switch
                    {
                        (false, false) => CollectionContains.Equal,
                        (true, false) => CollectionContains.Superset,
                        (false, true) => CollectionContains.Subset,
                        _ => Equals(A.Current, B.Current) ? Get(A.MoveNext(), B.MoveNext()) : CollectionContains.NotMatter
                    };
                return Get(A.MoveNext(), B.MoveNext());
            }
            #endregion
            return IsRelational ? FollowRelational() : NotRelational();
        }
        #endregion
        #region 判断包含关系是否为子集
        /// <summary>
        /// 判断某一包含关系是否为子集
        /// </summary>
        /// <param name="collectionContains">待判断的包含关系</param>
        /// <returns>如果<paramref name="collectionContains"/>为
        /// <see cref="CollectionContains.Equal"/>或<see cref="CollectionContains.Subset"/>，
        /// 则返回<see langword="true"/>，否则返回<see langword="false"/></returns>
        public static bool IsSubset(this CollectionContains collectionContains)
            => collectionContains is
            CollectionContains.Equal or CollectionContains.Subset;
        #endregion
        #endregion
        #region 改变一个元素，直到集合中没有重复的元素
        #region 可适用于任何集合
        /// <summary>
        /// 如果一个元素存在于某个集合中，则不断地改变它，
        /// 直到集合中没有这个元素为止，
        /// 这个方法可以为向某些不可重复的集合中添加元素提供方便
        /// </summary>
        /// <typeparam name="Ret">元素类型</typeparam>
        /// <param name="List">要检查的集合</param>
        /// <param name="Obj">要检查的元素</param>
        /// <param name="Change">如果集合中存在这个元素，则执行这个委托，
        /// 返回一个新元素，委托的第一个参数是原始元素，第二个参数是尝试的次数，从2开始</param>
        /// <returns></returns>
        public static Ret Distinct<Ret>(this IEnumerable<Ret> List, Ret Obj, Func<Ret, int, Ret> Change)
        {
            /*注释：Repeat从2开始的原因在于：
               在某些情况下能够比较方便的重命名重复元素，
               例如：在Excel中发现了重复的工作表，
               这时就可以直接将其命名为工作表（2）*/
            var Hash = List.ToHashSet();
            var Repeat = 2;
            var Obj2 = Obj;
            while (Hash.Contains(Obj))
                Obj = Change(Obj2, Repeat++);
            return Obj;
        }
        #endregion
        #region 仅适用于String集合
        /// <summary>
        /// 如果一个<see cref="string"/>存在于某个集合中，则不断地改变它，
        /// 直到集合中没有这个<see cref="string"/>为止，
        /// 改变的方法为将原始文本转化为Text(1)，Text(2)的形式
        /// </summary>
        /// <param name="List">文本所在的集合</param>
        /// <param name="Text">要检查的文本</param>
        /// <returns></returns>
        public static string Distinct(this IEnumerable<string> List, string Text)
            => List.Distinct(Text, (x, y) => $"{x}({y})");
        #endregion
        #endregion
        #region 返回一个集合的极限
        /// <summary>
        /// 返回一个集合的极限，也就是根据一个函数，
        /// 从元素中提取出键，然后返回键最大或者最小的元素
        /// </summary>
        /// <typeparam name="Ret">集合元素的类型，也是返回值类型</typeparam>
        /// <typeparam name="Key">键的类型，方法会比较键的大小，而不是<typeparamref name="Ret"/>的大小</typeparam>
        /// <param name="List">要返回极限的集合</param>
        /// <param name="Del">从元素中提取键的函数</param>
        /// <param name="RetMax">如果这个值为<see langword="true"/>，返回集合的最大值，为<see langword="false"/>，返回最小值</param>
        /// <param name="Comparison">用来比较键的比较器，如果为<see langword="null"/>，则默认为该类型的默认比较器</param>
        /// <returns></returns>
        public static Ret Limit<Ret, Key>(this IEnumerable<Ret> List, Func<Ret, Key> Del, bool RetMax, Func<Key, Key, int>? Comparison = null)
        {
            Comparison ??= Comparer<Key>.Default.Compare;
            return List.Aggregate((x, y) =>
            Comparison(
                Del(x), Del(y)) > 0 == RetMax ?         //根据x是否比y大，以及需要最大值或最小值，获取正确的结果
                x : y);
        }
        /*注释：算法为：
          每次检查集合中的两个元素，
          返回其中较大的一个，
          不断迭代直到集合末尾
           
          虽然也可以直接排序，
          然后取排序集合的第一位或最后一位，
          但是这个算法只需要遍历集合一次，
          性能有巨大的提高*/
        #endregion
    }
    #region 集合包含关系枚举
    /// <summary>
    /// 这个枚举指示两个集合之间的包含关系
    /// </summary>
    public enum CollectionContains
    {
        /// <summary>
        /// 表示真超集
        /// </summary>
        Superset,
        /// <summary>
        /// 表示真子集
        /// </summary>
        Subset,
        /// <summary>
        /// 表示两个集合完全等价
        /// </summary>
        Equal,
        /// <summary>
        /// 表示两个集合之间没有任何包含关系
        /// </summary>
        NotMatter
    }
    #endregion
}
