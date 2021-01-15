using System.Collections;
using System.Collections.Generic;
using System.Maths;

namespace System.Linq
{
    public static partial class ExtenIEnumerable
    {
        //所有关于集合的转化的方法，全部放在这个类型中

        #region 关于排序
        #region 默认排序
        /// <summary>
        /// 按照默认排序条件，对一个泛型集合进行排序
        /// </summary>
        /// <typeparam name="Obj">泛型集合的类型</typeparam>
        /// <param name="obj">要排序的泛型集合</param>
        /// <param name="IsAscending">如果这个值为<see langword="true"/>，则升序排列，为<see langword="false"/>，则降序排列</param>
        /// <returns></returns>
        public static Obj[] Sort<Obj>(this IEnumerable<Obj> obj, bool IsAscending = true)
            => obj.Sort(x => x, IsAscending);
        #endregion
        #region 可以输入任何排序条件
        /// <summary>
        /// 从一个集合中提取键，并对其进行排序，可以自行指定升序和降序
        /// </summary>
        /// <typeparam name="RetList">要排序的元素类型</typeparam>
        /// <typeparam name="Key">提取的键类型</typeparam>
        /// <param name="obj">需要进行排序的集合</param>
        /// <param name="del">从集合中提取，用于排序的键的委托</param>
        /// <param name="IsAscending">如果这个值为<see langword="true"/>，则升序排列，为<see langword="false"/>，则降序排列</param>
        /// <param name="Secondary">这个数组可通过委托获取次要排序条件，前面的优先级更高</param>
        /// <returns></returns>
        public static RetList[] Sort<RetList, Key>(this IEnumerable<RetList> obj, Func<RetList, Key> del, bool IsAscending = true, params Func<RetList, object>[] Secondary)
        {
            var list = IsAscending ? obj.OrderBy(del) : obj.OrderByDescending(del);
            foreach (var item in Secondary)
            {
                list = IsAscending ? list.ThenBy(item) : list.ThenByDescending(item);
            }
            return list.ToArray();
        }
        #endregion
        #region 将集合洗牌
        /// <summary>
        /// 打乱一个集合的顺序，并返回一个新的集合
        /// </summary>
        /// <typeparam name="RetList">集合中的元素类型</typeparam>
        /// <param name="List">要打乱顺序的集合</param>
        /// <param name="Rand">用来生成随机数的对象，如果为<see langword="null"/>，则使用一个默认对象</param>
        /// <returns></returns>
        public static RetList[] SortRand<RetList>(this IEnumerable<RetList> List, IRandom? Rand = null)
        {
            Rand ??= CreateBaseMathObj.RandomOnly;
            var arry = List.ToArray();
            for (int i = arry.Length - 1; i > 0; i--)
            {
                var RandIndex = Rand.RandRange(i + 1);
                var item = arry[i];
                arry[i] = arry[RandIndex];
                arry[RandIndex] = item;
            }
            return arry;
        }
        #endregion
        #endregion
        #region 关于并集
        #region 生成并返回多个集合的并集
        #region 以多个元素为参数
        /// <summary>
        /// 返回一个集合和多个元素的并集
        /// </summary>
        /// <typeparam name="Obj">集合元素的类型</typeparam>
        /// <param name="List">要形成并集的集合</param>
        /// <param name="Distinct">如果这个值为<see langword="true"/>，还会去除重复的元素</param>
        /// <param name="Elements">要合并的元素</param>
        /// <returns></returns>
        public static IEnumerable<Obj> Union<Obj>(this IEnumerable<Obj> List, bool Distinct, params Obj[] Elements)
            => List.Union(Distinct, new[] { Elements });
        #endregion
        #region 以多个集合为参数
        /// <summary>
        /// 生成并返回多个集合的并集
        /// </summary>
        /// <typeparam name="Obj">集合元素的类型</typeparam>
        /// <param name="List">第一个集合</param>
        /// <param name="Distinct">如果这个值为<see langword="true"/>，还会去除重复的元素</param>
        /// <param name="OtList">要合并的其他集合，可以是一个，也可以是多个</param>
        /// <returns></returns>
        public static IEnumerable<Obj> Union<Obj>(this IEnumerable<Obj> List, bool Distinct, params IEnumerable<Obj>[] OtList)
        {
            #region 本地函数
            IEnumerable<Obj> Get()
            {
                foreach (var i in List)
                    yield return i;
                foreach (var list in OtList)
                    foreach (var i in list)
                        yield return i;
            }
            #endregion
            return Distinct ? Get().Distinct() : Get();
        }
        #endregion
        #region 以一个嵌套集合作为参数
        /// <summary>
        /// 生成并返回一个嵌套集合中所有子集合的并集，
        /// </summary>
        /// <typeparam name="Obj">集合元素的类型</typeparam>
        /// <param name="List">一个包含所有要合并元素的嵌套集合</param>
        /// <param name="Distinct">如果这个值为<see langword="true"/>，还会去除重复的元素</param>
        /// <returns></returns>
        public static IEnumerable<Obj> UnionNesting<Obj>(this IEnumerable<IEnumerable<Obj>> List, bool Distinct)
        {
            #region 本地函数
            IEnumerable<Obj> Get()
            {
                foreach (var list in List)
                    foreach (var i in list)
                        yield return i;
            }
            #endregion 
            return Distinct ? Get().Distinct() : Get();
        }
        #endregion
        #endregion
        #region 将多个集合的元素添加到一个集合
        /// <summary>
        /// 将多个集合的所有元素添加到一个集合，但是不返回新对象，注意：
        /// 这个方法不会去除重复的元素
        /// </summary>
        /// <typeparam name="Obj">集合的元素集合</typeparam>
        /// <param name="List">所有其他集合的元素会被添加到这个集合</param>
        /// <param name="OtList">要与第一个集合合并的其他集合</param>
        public static void UnionAdd<Obj>(this ICollection<Obj> List, params IEnumerable<Obj>[] OtList)
            => OtList.ForEach(x => x.ForEach(List.Add));
        #endregion
        #endregion
        #region 关于拆分集合
        #region 简单拆分
        /// <summary>
        /// 将一个集合拆分，并返回拆分后的集合
        /// </summary>
        /// <typeparam name="Obj">集合的元素类型</typeparam>
        /// <param name="List">要拆分的集合</param>
        /// <param name="Num">指示集合拆分的程度，也就是子集合的元素多寡</param>
        /// <param name="NumIsCount">如果这个值为<see langword="true"/>，则<paramref name="Num"/>参数指的是每个子集合应该有多少个元素，
        /// 如果这个值为<see langword="false"/>，则<paramref name="Num"/>参数指的是应该将父集合拆分成多少个子集合</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<Obj>> Split<Obj>(this IEnumerable<Obj> List, int Num, bool NumIsCount)
        {
            #region 用于拆分集合的本地函数
            static IEnumerable<IEnumerable<Obj>> Get(IEnumerable<Obj> objs, int Count)
            {
                using var Enumerator = objs.GetEnumerator();
                var list = new LinkedList<Obj>();
                for (int i = 1; true; i++)
                {
                    var NotObj = !Enumerator.MoveNext();
                    if (!NotObj)
                        list.AddLast(Enumerator.Current);
                    if (NotObj || i % Count == 0)
                    {
                        if (list.Any())
                            yield return list;
                        if (NotObj)
                            yield break;
                        list = new LinkedList<Obj>();
                    }
                }
            }
            #endregion
            ExceptionIntervalOut.Check(1, null, Num);
            if (NumIsCount)
                return Get(List, Num);
            var arry = List.ToArray();
            var len = arry.Length;
            return Get(arry, (len + len % Num) / Num);
        }

        /*说明文档
          #当NumIsCount为True时，
          本函数会延迟返回，不会将List转换为数组，性能更高*/
        #endregion
        #region 按索引拆分
        /// <summary>
        /// 按照指定的索引拆分集合
        /// </summary>
        /// <typeparam name="Obj">集合的元素类型</typeparam>
        /// <param name="List">待拆分的集合</param>
        /// <param name="Index">这些索引指示拆分集合的位置</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<Obj>> Split<Obj>(this IEnumerable<Obj> List, params int[] Index)
        {
            using var Enumerator = List.GetEnumerator();
            var list = new LinkedList<Obj>();
            for (int i = 0, ArryIndex = 0; true; i++)
            {
                var NotObj = !Enumerator.MoveNext();
                if (NotObj || i == Index[ArryIndex])
                {
                    yield return list;
                    if (NotObj)
                        yield break;
                    list = new LinkedList<Obj>();
                    ArryIndex = Math.Min(Index.Length - 1, ArryIndex + 1);
                }
                list.AddLast(Enumerator.Current);
            }
        }

        /*说明文档：
          假设有集合{1，2，3，4，5}，
          Index参数填入{1，3}，
          那么函数会返回3个集合，分别是
          {1}
          {2,3}
          {4,5}*/
        #endregion
        #region 按条件拆分
        /// <summary>
        /// 按照条件，将一个集合拆分成两部分，
        /// 分别是满足条件和不满足条件的部分
        /// </summary>
        /// <typeparam name="Obj">要拆分的集合元素类型</typeparam>
        /// <param name="List">要拆分的集合</param>
        /// <param name="Del">对集合元素进行判断的委托</param>
        /// <returns></returns>
        public static (IEnumerable<Obj> IsTrue, IEnumerable<Obj> IsFalse) Split<Obj>(this IEnumerable<Obj> List, Func<Obj, bool> Del)
        {
            var (IsTrue, IsFalse) = (new LinkedList<Obj>(), new LinkedList<Obj>());
            foreach (var i in List)
            {
                var list = Del(i) ? IsTrue : IsFalse;
                list.AddLast(i);
            }
            return (IsTrue, IsFalse);
        }
        #endregion
        #endregion
        #region 关于Zip
        #region 合并两个集合
        /// <summary>
        /// 合并两个集合的对应元素，与<see cref="Enumerable.Zip{TFirst, TSecond, TResult}(IEnumerable{TFirst}, IEnumerable{TSecond}, Func{TFirst, TSecond, TResult})"/>不同的是，
        /// 如果两个集合的元素数量不相等，
        /// 则会用一个默认值来填补缺失的元素
        /// </summary>
        /// <typeparam name="ObjA">第一个集合的元素类型</typeparam>
        /// <typeparam name="ObjB">第二个集合的元素类型</typeparam>
        /// <typeparam name="ObjC">结果集合的元素类型</typeparam>
        /// <param name="ListA">要合并的第一个集合</param>
        /// <param name="ListB">要合并的第二个集合</param>
        /// <param name="Del">这个委托传入两个集合的元素，并返回结果</param>
        /// <param name="DefA">如果第一个集合的元素数量比另一个集合小，则缺失的元素通过这个委托生成</param>
        /// <param name="DefB">如果第二个集合的元素数量比另一个集合小，则缺失的元素通过这个委托生成</param>
        /// <returns></returns>
        public static IEnumerable<ObjC> ZipFill<ObjA, ObjB, ObjC>(this IEnumerable<ObjA> ListA, IEnumerable<ObjB> ListB,
            Func<ObjA, ObjB, ObjC> Del, Func<ObjA>? DefA = null, Func<ObjB>? DefB = null)
        {
            #region 本地函数
            IEnumerable<ObjC> Get(IEnumerable<ObjA> A, IEnumerable<ObjB> B)
                => A.Zip(B, Del);
            #endregion
            var (AC, BC) = (ListA.Count(), ListB.Count());
            if (AC == BC)
                return Get(ListA, ListB);
            return AC < BC ?
            Get(ListA.Fill(BC, DefA), ListB) :
            Get(ListA, ListB.Fill(AC, DefB));

        }
        #endregion
        #region 将两个集合合并为元组
        /// <summary>
        /// 将两个集合的元素合并为元组
        /// </summary>
        /// <typeparam name="ObjA">第一个集合的元素类型</typeparam>
        /// <typeparam name="ObjB">第二个集合的元素类型</typeparam>
        /// <param name="ListA">要合并的第一个集合</param>
        /// <param name="ListB">要合并的第二个集合</param>
        /// <param name="Truncated">在两个集合元素数量不相等的情况下，
        /// 如果这个值为<see langword="true"/>，则丢弃多余的元素，为<see langword="false"/>，则将不足的元素用默认值填补</param>
        /// <returns></returns>
        public static IEnumerable<(ObjA A, ObjB B)> Zip<ObjA, ObjB>(this IEnumerable<ObjA> ListA, IEnumerable<ObjB> ListB, bool Truncated = true)
        {
            static (ObjA, ObjB) Get(ObjA x, ObjB y)
                => (x, y);
            return Truncated ? ListA.Zip(ListB, Get) : ListA.ZipFill(ListB, Get);
        }
        #endregion
        #endregion
        #region 填补集合的元素
        /// <summary>
        /// 将一个集合的元素填补至指定的数量，
        /// 如果该集合的元素数量大于应返回的元素数量，
        /// 则只返回集合前面的元素
        /// </summary>
        /// <typeparam name="Obj">集合元素的类型</typeparam>
        /// <param name="List">待填补的集合</param>
        /// <param name="Count">填补后的集合应有的元素数量</param>
        /// <param name="NewElements">如果元素需要填补，则通过这个委托获取新元素，
        /// 如果为<see langword="null"/>，则返回默认值</param>
        /// <returns></returns>
        public static IEnumerable<Obj> Fill<Obj>(this IEnumerable<Obj> List, int Count, Func<Obj>? NewElements = null)
        {
            foreach (var e in List)
            {
                if (Count-- > 0)
                    yield return e;
                else break;
            }
            while (Count-- > 0)
                yield return NewElements == null ? default! : NewElements.Invoke();
        }
        #endregion
        #region 将非泛型集合转化为泛型集合
        #region 转换为泛型迭代器
        /// <summary>
        /// 将非泛型迭代器转换为类型参数为<see cref="object"/>的泛型迭代器
        /// </summary>
        /// <param name="list">待转换的非泛型迭代器</param>
        /// <returns></returns>
        public static IEnumerable<object> OfType(this IEnumerable list)
            => list.OfType<object>();
        #endregion
        #region 将转换为List
        /// <summary>
        /// 将任意集合转换为<see cref="List{T}"/>
        /// </summary>
        /// <typeparam name="Obj"><see cref="List{T}"/>元素的类型</typeparam>
        /// <param name="list">要转换的集合，可以是最低级的<see cref="IEnumerable"/></param>
        /// <returns></returns>
        public static List<Obj> ToList<Obj>(this IEnumerable list)
            => list.OfType<Obj>().ToList();
        #endregion
        #region 转换为数组
        /// <summary>
        /// 将任意集合转换为数组
        /// </summary>
        /// <typeparam name="Obj">数组的元素类型</typeparam>
        /// <param name="list">待转换的集合</param>
        /// <returns></returns>
        public static Obj[] ToArray<Obj>(this IEnumerable list)
            => list.OfType<Obj>().ToArray();
        #endregion
        #endregion
    }
}
