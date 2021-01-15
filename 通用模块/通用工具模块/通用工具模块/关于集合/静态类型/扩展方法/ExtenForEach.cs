using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class ExtenIEnumerable
    {
        //所有关于集合的遍历与累加的方法，全部放在这个类型中

        #region 关于遍历
        #region 遍历泛型集合
        /// <summary>
        /// 遍历一个泛型集合
        /// </summary>
        /// <typeparam name="Obj">要遍历的元素类型</typeparam>
        /// <param name="list">要遍历的泛型集合</param>
        /// <param name="del">对每个元素执行的委托</param>
        public static void ForEach<Obj>(this IEnumerable<Obj> list, Action<Obj> del)
        {
            foreach (Obj i in list)
                del(i);
        }
        #endregion
        #region 拆分遍历
        /// <summary>
        /// 分别遍历一个迭代器的第一个元素和其他元素
        /// </summary>
        /// <typeparam name="Obj">迭代器的元素类型</typeparam>
        /// <param name="list">待遍历的迭代器</param>
        /// <param name="first">用来遍历第一个元素的委托，
        /// 它的第一个参数是待遍历的元素，第二个参数就是<paramref name="other"/>的柯里化形式，参数传入第一个元素</param>
        /// <param name="other">用来遍历其他元素的委托</param>
        public static void ForEachSplit<Obj>(this IEnumerable<Obj> list, Action<Obj, Action> first, Action<Obj> other)
        {
            var (First, Other, HasElements) = list.First(false);
            if (HasElements)
            {
                first(First, () => other(First));
                Other.ForEach(other);
            }
        }
        #endregion
        #endregion
        #region 关于累加
        #region 关于Sum
        #region 可以对任何对象使用
        /// <summary>
        /// 返回一个序列的和，可以对任何对象使用，
        /// 只要它重载了加法运算符， 或使用委托指定了计算加法的方式
        /// </summary>
        /// <typeparam name="Obj">集合元素和返回值的类型</typeparam>
        /// <param name="List">要计算总和的集合</param>
        /// <param name="Add">这个委托指定了计算加法的方式，
        /// 如果为<see langword="null"/>，则尝试调用自带的加法运算符</param>
        /// <returns></returns>
        public static Obj Sum<Obj>(this IEnumerable<Obj> List, Func<Obj, Obj, Obj>? Add = null)
        {
            var arry = List.ToArray();
            Add ??= new Func<Obj, Obj, Obj>((x, y) => (dynamic?)x + y);
            return arry.Length == 1 ? arry[0] : arry.Aggregate(Add);
        }
        #endregion
        #endregion
        #region 聚合函数
        /// <summary>
        /// 将集合中所有相邻的元素聚合起来，并返回一个新集合，
        /// 如果集合的元素小于2，则返回一个空集合
        /// </summary>
        /// <typeparam name="Obj">集合的元素类型</typeparam>
        /// <typeparam name="Ret">返回值类型</typeparam>
        /// <param name="List">要聚合的集合</param>
        /// <param name="Del">这个委托的第一个参数是两个相邻元素的左边，
        /// 第二个元素是相邻元素的右边，返回值是聚合后的新元素</param>
        /// <returns></returns>
        public static IEnumerable<Ret> Polymerization<Obj, Ret>(this IEnumerable<Obj> List, Func<Obj, Obj, Ret> Del)
        {
            var (left, Other, HasElements) = List.First(false);
            if (HasElements)
            {
                foreach (var right in Other)
                {
                    yield return Del(left, right);
                    left = right;
                }
            }
        }
        #endregion
        #region 关于Aggregate
        #region 带转换，且可以访问种子的累加
        /// <summary>
        /// 将一个集合的元素转化为另一种类型，
        /// 在转换的时候，可以访问一个种子作为参考
        /// </summary>
        /// <typeparam name="Source">原始集合的元素类型</typeparam>
        /// <typeparam name="Seed">种子的类型</typeparam>
        /// <typeparam name="Ret">返回值的类型</typeparam>
        /// <param name="List">待转换的原始集合</param>
        /// <param name="Initial">种子的初始值</param>
        /// <param name="Del">这个委托的参数是集合元素和当前的种子，
        /// 并返回一个元组，分别是转换后的新元素，以及经过累加后的新种子</param>
        /// <returns></returns>
        public static IEnumerable<Ret> AggregateSelect<Source, Seed, Ret>(this IEnumerable<Source> List, Seed Initial, Func<Source, Seed, (Ret, Seed)> Del)
        {
            foreach (var item in List)
            {
                var (RetIten, NewSeed) = Del(item, Initial);
                Initial = NewSeed;
                yield return RetIten;
            }
        }
        #endregion
        #endregion
        #endregion
        #region 关于返回首位元素
        /// <summary>
        /// 传入一个集合，然后返回一个元组，
        /// 它的项分别是集合的第一个元素，剩下的元素，以及集合中是否存在元素
        /// </summary>
        /// <typeparam name="Obj">集合的元素类型</typeparam>
        /// <param name="List">待返回元素的集合</param>
        /// <param name="Check">在集合没有任何元素的情况下，
        /// 如果这个值为<see langword="true"/>，则引发异常，否则返回默认值</param>
        /// <returns></returns>
        public static (Obj First, IEnumerable<Obj> Other, bool HasElements) First<Obj>(this IEnumerable<Obj> List, bool Check)
        {
            var Enumerator = List.GetEnumerator();
            var HasElements = Enumerator.MoveNext();
            var First = HasElements ? Enumerator.Current :
                Check ? throw new Exception("该集合没有任何元素") : default(Obj)!;
            #region 本地函数
            IEnumerable<Obj> Get()
            {
                while (Enumerator.MoveNext())
                {
                    yield return Enumerator.Current;
                }
                Enumerator.Dispose();
            }
            #endregion 
            return (First, Get(), HasElements);
        }
        #endregion
        #region 如果集合不存在任何元素，则抛出异常
        /// <summary>
        /// 如果集合中不存在任何元素，则抛出一个异常
        /// </summary>
        /// <param name="List">待检查的集合</param>
        /// <param name="Describe">对集合的描述，
        /// 这个参数可以更清楚地告诉调试者，这个集合有什么作用</param>
        public static void AnyCheck(this IEnumerable List, string? Describe = null)
        {
            if (!List.GetEnumerator().MoveNext())
                throw new ArgumentException(Describe + "集合没有任何元素");
        }
        #endregion
        #region 返回迭代器的缓存
        #region 缓存同步迭代器
        /// <summary>
        /// 返回一个迭代器的缓存
        /// </summary>
        /// <typeparam name="Obj">迭代器的元素类型</typeparam>
        /// <param name="list">要返回缓存的迭代器</param>
        /// <param name="CacheAll">如果这个值为<see langword="true"/>，表示在获取第一个元素时缓存全部元素，
        /// 否则代表逐个缓存元素，正确指定这个参数可以改善性能</param>
        /// <returns>一个新的迭代器，它仍然通过延迟迭代返回元素，
        /// 但是在遍历过一次以后，这些元素会被缓存起来</returns>
        public static IEnumerable<Obj> Cache<Obj>(this IEnumerable<Obj> list, bool CacheAll = false)
            => new EnumerableCacheRealize<Obj>(list, CacheAll);
        #endregion
        #region 缓存异步迭代器
        /// <summary>
        /// 返回一个异步迭代器的缓存
        /// </summary>
        /// <typeparam name="Obj">异步迭代器的元素类型</typeparam>
        /// <param name="list">要返回缓存的异步迭代器</param>
        /// <param name="CacheAll">如果这个值为<see langword="true"/>，表示在获取第一个元素时缓存全部元素，
        /// 否则代表逐个缓存元素，正确指定这个参数可以改善性能</param>
        /// <returns>一个新的迭代器，它仍然通过延迟迭代返回元素，
        /// 但是在遍历过一次以后，这些元素会被缓存起来</returns>
        public static IEnumerable<Obj> Cache<Obj>(this IAsyncEnumerable<Obj> list, bool CacheAll = false)
            => list.ToEnumerable().Cache(CacheAll);
        #endregion
        #endregion
    }
}
