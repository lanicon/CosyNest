using System.Collections.Generic;

namespace System.Linq
{
    /// <summary>
    /// 所有关于集合的扩展方法全部放在这个类中，通常无需专门调用
    /// </summary>
    public static partial class ExtenIEnumerable
    {
        //所有关于集合的操作的方法，全部放在这个类型中

        #region 对集合的操作
        #region 关于添加或删除元素
        #region 关于添加元素
        #region 批量添加元素
        /// <summary>
        /// 向一个集合中批量添加元素，由于C#7.2的新语法，
        /// 这个方法可以让集合能够直接用另一个集合进行初始化
        /// </summary>
        /// <typeparam name="Obj">集合元素的类型</typeparam>
        /// <param name="List">要添加元素的集合</param>
        /// <param name="AddList">被添加进集合的元素</param>
        public static void Add<Obj>(this ICollection<Obj> List, IEnumerable<Obj> AddList)
            => AddList.ForEach(x => List.Add(x));

        /*“可以用集合初始化集合”指的是：
            new List<int>()
            {
                new List<int>{1}
            };
        */
        #endregion
        #endregion
        #region 关于删除元素
        #region 移除所有符合条件的元素
        /// <summary>
        /// 移除集合中所有符合条件的元素
        /// </summary>
        /// <typeparam name="Obj">集合的元素类型</typeparam>
        /// <param name="List">要移除元素的集合</param>
        /// <param name="Del">如果这个委托返回<see langword="true"/>，则将该元素移除</param>
        public static void RemoveWhere<Obj>(this ICollection<Obj> List, Func<Obj, bool> Del)
        {
            #region 本地函数
            #region 适用于IList
            void RemoveIList(IList<Obj> list)
            {
                var index = list.PackIndex().
                    Where(x => Del(x.Elements)).
                    Select(x => x.Index).Reverse().ToArray();
                index.ForEach(x => list.RemoveAt(x));
            }
            #endregion
            #region 适用于ICollection
            void RemoveICollection()
            {
                var Rem = List.Where(Del).ToArray();
                Rem.ForEach(x => List.Remove(x));
            }
            #endregion
            #endregion
            switch (List)
            {
                case List<Obj> L:
                    L.RemoveAll(x => Del(x)); break;
                case IList<Obj> L:
                    RemoveIList(L); break;
                default:
                    RemoveICollection(); break;
            }
        }

        /*说明文档：
           本函数经过优化，如果发现集合是List，或实现了IList接口，
           则会调用效率更高的方法来移除元素，
           经测试，集合越大带来的性能提升越明显，
           但如果集合很小，性能会略微下降*/
        #endregion
        #endregion
        #endregion
        #endregion
        #region 关于索引（Int形式）
        #region 按照元素返回索引
        /// <summary>
        /// 在任意泛型集合中，根据元素返回索引，不需要实现<see cref="IList{T}"/>，
        /// 如果没有找到，返回-1
        /// </summary>
        /// <typeparam name="Obj">泛型集合的类型</typeparam>
        /// <param name="List">元素所在的泛型集合</param>
        /// <param name="obj">要检查索引的元素</param>
        /// <param name="Optimization">如果这个值为<see langword="true"/>，
        /// 则当<paramref name="List"/>实现<see cref="IList{T}"/>时，调用<see cref="IList{T}.IndexOf(T)"/>以提高性能，
        /// 如果本方法在<see cref="IList{T}.IndexOf(T)"/>中调用，请传入<see langword="false"/>以避免无限递归异常</param>
        /// <returns></returns>
        public static int BinarySearch<Obj>(this IEnumerable<Obj> List, Obj obj, bool Optimization = true)
        {
            if (List is IList<Obj> L && Optimization)
                return L.IndexOf(obj);
            foreach (var (e, index, _) in List.PackIndex())
            {
                if (Equals(e, obj))
                    return index;
            }
            return -1;
        }
        #endregion
        #region 封装集合的元素和索引
        /// <summary>
        /// 封装一个集合的元素，索引，和元素数量，并返回一个新集合
        /// </summary>
        /// <typeparam name="Obj">集合的元素类型</typeparam>
        /// <param name="List">待转换的集合</param>
        /// <param name="GetCount">如果这个值为<see langword="true"/>，则会获取集合元素的数量，但是这会影响性能，
        /// 如果为<see langword="false"/>，则不会这样做，返回的元组的Count字段为-1</param>
        /// <returns></returns>
        public static IEnumerable<(Obj Elements, int Index, int Count)> PackIndex<Obj>(this IEnumerable<Obj> List, bool GetCount = false)
        {
            var Index = 0;
            var Count = GetCount ? List.Count() : -1;
            foreach (var obj in List)
            {
                yield return (obj, Index++, Count);
            }
        }
        #endregion
        #endregion 
        #region 关于Index和Range
        #region 关于Range
        #region 解构范围
        /// <summary>
        /// 将范围解构为开始和结束索引
        /// </summary>
        /// <param name="R">待解构的范围</param>
        /// <param name="Begin">开始索引</param>
        /// <param name="End">结束索引</param>
        public static void Deconstruct(this Range R, out Index Begin, out Index End)
        {
            Begin = R.Start;
            End = R.End;
        }
        #endregion
        #region 返回范围的开始和结束
        #region 传入集合的长度
        /// <summary>
        /// 返回范围的开始和结束
        /// </summary>
        /// <param name="Range">待返回开始和结束的范围</param>
        /// <param name="Length">集合元素的数量</param>
        /// <returns></returns>
        public static (int Begin, int End) GetOffsetAndEnd(this Range Range, int Length)
        {
            var (Beg, Len) = Range.GetOffsetAndLength(Length);
            return (Beg, Beg + Len - 1);
        }
        #endregion
        #region 传入集合
        /// <summary>
        /// 根据一个集合的元素数量，计算范围的开始和结束
        /// </summary>
        /// <typeparam name="Obj">集合元素的类型</typeparam>
        /// <param name="Range">待返回开始和结束的范围</param>
        /// <param name="List">用来计算元素数量的集合</param>
        /// <returns></returns>
        public static (int Begin, int End) GetOffsetAndEnd<Obj>(this Range Range, IEnumerable<Obj> List)
        {
            var (B, E) = Range;
            if (!B.IsFromEnd && !E.IsFromEnd)
                return (B.Value, E.Value);
            var Count = List.Count();           //即便开始和结束索引都是倒着数的，也只需要计算一次元素数量
            return (B.GetOffset(Count), E.GetOffset(Count));
        }
        #endregion
        #endregion
        #region 返回是否为确定范围
        /// <summary>
        /// 返回一个范围是否为确定范围，
        /// 也就是它的<see cref="Range.End"/>是否从集合开头数起，
        /// 确定范围不会随着集合元素的增减而发生变化
        /// </summary>
        /// <param name="R">待确定的范围</param>
        /// <returns></returns>
        public static bool IsAccurate(this Range R)
            => !R.End.IsFromEnd;
        #endregion
        #endregion
        #region 关于返回元素
        #region 按索引返回元素
        /// <summary>
        /// 按照索引返回元素，不需要实现<see cref="IList{T}"/>
        /// </summary>
        /// <typeparam name="Obj">集合元素的类型</typeparam>
        /// <param name="List">要返回元素的集合</param>
        /// <param name="index">用来提取元素的索引</param>
        /// <param name="Thrown">在索引非法的时候，如果这个值为<see langword="true"/>，则抛出异常，否则会返回一个默认值</param>
        /// <param name="Crossed">在索引非法时，会通过这个延迟对象获取默认返回值</param>
        /// <returns></returns>
        public static Obj? ElementAt<Obj>(this IEnumerable<Obj> List, Index index, bool Thrown = true, LazyPro<Obj>? Crossed = null)
        {
            try
            {
                return List is IList<Obj> L ?
                    L[index] : List.ElementAt(index.GetOffset(List));
            }
            catch (ArgumentOutOfRangeException) when (!Thrown)
            {
                return Crossed;
            }
        }
        #endregion
        #region 按范围返回元素
        /// <summary>
        /// 按照范围，返回集合中的元素
        /// </summary>
        /// <typeparam name="Obj">集合元素的类型</typeparam>
        /// <param name="List">待返回元素的集合</param>
        /// <param name="Range">返回元素的范围</param>
        /// <param name="Thrown">在范围非法的时候，如果这个值为<see langword="true"/>，会抛出异常，否则会返回一个空数组</param>
        /// <returns></returns>
        public static Obj[] ElementAt<Obj>(this IEnumerable<Obj> List, Range Range, bool Thrown = true)
        {
            try
            {
                return (List is Obj[] Arry ? Arry : List.ToArray())[Range];
            }
            catch (ArgumentOutOfRangeException) when (!Thrown)
            {
                return Array.Empty<Obj>();
            }
        }
        #endregion
        #endregion
        #region 关于Index
        #region 根据集合计算索引
        /// <summary>
        /// 根据集合，计算出索引的实际值
        /// </summary>
        /// <typeparam name="Obj">集合的元素类型</typeparam>
        /// <param name="index">待计算的索引</param>
        /// <param name="List">索引所指向的集合</param>
        /// <returns></returns>
        public static int GetOffset<Obj>(this Index index, IEnumerable<Obj> List)
            => index.IsFromEnd ? index.GetOffset(List.Count()) : index.Value;
        #endregion
        #region 转换为Range
        /// <summary>
        /// 将一个<see cref="Index"/>转换为<see cref="Range"/>，
        /// 它仅选取集合中的一个元素
        /// </summary>
        /// <param name="index">待转换的<see cref="Index"/></param>
        /// <returns></returns>
        public static Range ToRange(this Index index)
        {
            if (index.Equals(^0))
                throw new ArgumentException($"{index}为^0，它本身不在集合中，不能为它创建范围");
            var i = index.Value + (index.IsFromEnd ? -1 : 1);
            return new Range(index, new Index(i, index.IsFromEnd));
        }
        #endregion
        #endregion
        #endregion
    }
}
