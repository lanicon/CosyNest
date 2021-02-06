using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    /// <summary>
    /// 所有关于集合的扩展方法全部放在这个类中，通常无需专门调用
    /// </summary>
    public static partial class ExtenIEnumerable
    {
        /*所有关于集合的操作的方法，全部放在这个部分类中，
          操作指的是：API不返回任何值，或者改变了集合的状态*/
        
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
    }
}
