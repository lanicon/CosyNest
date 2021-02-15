using System.Collections.Generic;

namespace System.Linq
{
    public static partial class ExtenIEnumerable
    {
        /*这个分部类专门储存为特定集合类型优化过的扩展方法，
          它们耦合更强，但是具有更高的性能*/

        #region 为ICollection<T>优化
        #region 合并ICollection<T>
        /// <summary>
        /// 取多个<see cref="ICollection{T}"/>的并集
        /// </summary>
        /// <typeparam name="Obj">集合的元素类型</typeparam>
        /// <param name="collection">原始集合</param>
        /// <param name="collectionOther">要合并的其他集合</param>
        /// <returns></returns>
        public static Obj[] Union<Obj>(this ICollection<Obj> collection, params ICollection<Obj>[] collectionOther)
        {
            var len = collection.Count + collectionOther.Sum(x => x.Count);
            var newArray = new Obj[len];
            var pos = 0;
            foreach (var item in collectionOther)
            {
                item.CopyTo(newArray, pos);
                pos += item.Count;
            }
            return newArray;
        }
        #endregion
        #endregion
        #region 为IList<T>优化
        #region 比较IList<T>的值相等性
        /// <summary>
        /// 比较<see cref="IList{T}"/>的值相等性
        /// </summary>
        /// <typeparam name="Obj">集合的元素类型</typeparam>
        /// <param name="collectionA">要比较的第一个集合</param>
        /// <param name="collectionB">要比较的第二个集合</param>
        /// <returns>如果两个集合的长度和对应索引的元素完全一致，
        /// 则返回<see langword="true"/>，否则返回<see langword="false"/></returns>
        public static bool SequenceEqual<Obj>(this IList<Obj> collectionA, IList<Obj> collectionB)
        {
            var len = collectionA.Count;
            if (len != collectionB.Count)
                return false;
            for (int i = 0; i < len; i++)
            {
                if (!Equals(collectionA[i], collectionB[i]))
                    return false;
            }
            return true;
        }
        #endregion 
        #endregion
    }
}
