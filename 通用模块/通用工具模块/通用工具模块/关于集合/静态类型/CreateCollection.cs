using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Linq;

namespace System.Collections.Generic
{
    /// <summary>
    /// 这个静态类可以用来帮助创建集合
    /// </summary>
    public static class CreateCollection
    {
        #region 有关ITwoWayMap
        #region 直接创建双向映射表
        /// <summary>
        /// 创建一个双向映射表，并返回
        /// </summary>
        /// <typeparam name="A">要映射的第一个对象类型</typeparam>
        /// <typeparam name="B">要映射的第二个对象类型</typeparam>
        /// <returns></returns>
        public static ITwoWayMap<A, B> TwoWayMap<A, B>()
            where A : notnull
            where B : notnull
            => new TwoWayMap<A, B>();
        #endregion
        #region 创建双向映射表
        /// <summary>
        /// 构造双向映射表，
        /// 并将指定的双向映射添加到表中
        /// </summary>
        /// <typeparam name="A">要映射的A类型</typeparam>
        /// <typeparam name="B">要映射的B类型</typeparam>
        /// <param name="map">这些元组的项会互相映射</param>
        /// <returns></returns>
        public static ITwoWayMap<A, B> TwoWayMap<A, B>(params (A, B)[] map)
            where A : notnull
            where B : notnull
            => new TwoWayMap<A, B>(map);
        #endregion
        #endregion
        #region 有关环形迭代器
        /// <summary>
        /// 创建一个环形的迭代器，
        /// 当它迭代完最后一个元素以后，会重新迭代第一个元素
        /// </summary>
        /// <typeparam name="Obj">迭代器中的元素类型</typeparam>
        /// <param name="enumerable">环形迭代器的元素实际由这个迭代器提供<</param>
        /// <returns></returns>
        public static IEnumerable<Obj> Ring<Obj>(IEnumerable<Obj> enumerable)
        {
            #region 本地函数
            IEnumerable<Obj> Get()
            {
                var Enumerator = enumerable.GetEnumerator();
                if (Enumerator.MoveNext())                      //如果集合中没有元素，则停止迭代，不会死循环
                    yield return Enumerator.Current;
                else yield break;
                while (true)
                {
                    if (Enumerator.MoveNext())
                        yield return Enumerator.Current;
                    else Enumerator.Reset();
                }
            }
            #endregion
            return Get();
        }
        #endregion
        #region 有关IAddOnlyDictionary
        #region 直接创建
        /// <summary>
        /// 创建一个只能添加元素的字典，并返回
        /// </summary>
        /// <typeparam name="Key">字典的键类型</typeparam>
        /// <typeparam name="Value">字典的值类型</typeparam>
        /// <param name="canModify">如果这个值为<see langword="true"/>，
        /// 代表可以修改已经被添加到字典的值，否则代表禁止修改</param>
        /// <param name="dictionary">函数将使用这个字典存储键值对</param>
        /// <returns></returns>
        public static IAddOnlyDictionary<Key, Value> AddOnlyDictionary<Key, Value>(bool canModify = false, IDictionary<Key, Value>? dictionary = null)
            where Key : notnull
            => new AddOnlyDictionary<Key, Value>(dictionary, canModify);
        #endregion
        #region 创建线程安全字典
        /// <summary>
        /// 创建一个线程安全，且只能添加元素的字典，并返回
        /// </summary>
        /// <typeparam name="Key">字典的键类型</typeparam>
        /// <typeparam name="Value">字典的值类型</typeparam>
        ///  <param name="canModify">如果这个值为<see langword="true"/>，
        /// 代表可以修改已经被添加到字典的值，否则代表禁止修改</param>
        /// <returns></returns>
        public static IAddOnlyDictionary<Key, Value> AddOnlyDictionaryConcurrent<Key, Value>(bool canModify = false)
            where Key : notnull
            => AddOnlyDictionary(canModify, new ConcurrentDictionary<Key, Value>());
        #endregion
        #endregion
        #region 创建空数组
#pragma warning disable IDE0060
        /// <summary>
        /// 创建一个空数组，它与<see cref="Array.Empty{T}"/>唯一的不同在于，
        /// 数组的元素类型是通过推断得出的
        /// </summary>
        /// <typeparam name="Obj">数组的元素类型</typeparam>
        /// <param name="infer">这个参数不会被实际使用，
        /// 它的唯一目的在于推断数组的元素类型</param>
        /// <returns></returns>
        public static Obj[] Empty<Obj>(IEnumerable<Obj>? infer)
           => Array.Empty<Obj>();
#pragma warning restore
        #endregion
        #region 帮助创建NotifyCollectionChangedEventArgs 
        #region 创建描述添加或删除的Args 
        /// <summary>
        /// 帮助创建一个<see cref="NotifyCollectionChangedEventArgs"/>，
        /// 只能创建描述添加或删除的事件数据
        /// </summary>
        /// <param name="isAdd">如果这个值为<see langword="true"/>，创建描述添加的事件数据，
        /// 否则创建描述删除的事件数据</param>
        /// <param name="elements">受影响的对象，可以是集合（代表多项更改），
        /// 也可以是集合中的元素（代表单项更改）</param>
        /// <returns></returns>
        public static NotifyCollectionChangedEventArgs NCCE(bool isAdd, object elements)
        {
            var action = isAdd ? NotifyCollectionChangedAction.Add : NotifyCollectionChangedAction.Remove;
            IList list = elements is IEnumerable List ? List.ToList<object>() : new object[] { elements };
            return new(action, list);
        }
        #endregion
        #endregion
        #region 创建包含指定数量元素的迭代器
        /// <summary>
        /// 创建一个包含指定数量元素的迭代器
        /// </summary>
        /// <typeparam name="Obj">集合的元素类型</typeparam>
        /// <param name="count">集合的元素数量</param>
        /// <param name="getElements">用来生成元素的委托，委托参数就是元素的索引</param>
        /// <returns></returns>
        public static IEnumerable<Obj> Range<Obj>(int count, Func<int, Obj> getElements)
        {
            for (int i = 0; i < count; i++)
            {
                yield return getElements(i);
            }
        }
        #endregion
    }
}
