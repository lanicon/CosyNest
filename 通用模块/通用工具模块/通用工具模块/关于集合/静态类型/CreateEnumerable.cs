using System.Collections.Concurrent;

namespace System.Collections.Generic
{
    /// <summary>
    /// 这个静态类可以用来帮助创建集合
    /// </summary>
    public static class CreateEnumerable
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
        /// <param name="Map">这些元组的项会互相映射</param>
        /// <returns></returns>
        public static ITwoWayMap<A, B> TwoWayMap<A, B>(params (A, B)[] Map)
            where A : notnull
            where B : notnull
            => new TwoWayMap<A, B>(Map);
        #endregion
        #endregion
        #region 有关环形迭代器
        /// <summary>
        /// 创建一个环形的迭代器，
        /// 当它迭代完最后一个元素以后，会重新迭代第一个元素
        /// </summary>
        /// <typeparam name="Obj">迭代器中的元素类型</typeparam>
        /// <param name="Enumerable">环形迭代器的元素实际由这个迭代器提供<</param>
        /// <returns></returns>
        public static IEnumerable<Obj> Ring<Obj>(IEnumerable<Obj> Enumerable)
        {
            #region 本地函数
            IEnumerable<Obj> Get()
            {
                var Enumerator = Enumerable.GetEnumerator();
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
        /// <param name="CanModify">如果这个值为<see langword="true"/>，
        /// 代表可以修改已经被添加到字典的值，否则代表禁止修改</param>
        /// <param name="Dictionary">函数将使用这个字典存储键值对</param>
        /// <returns></returns>
        public static IAddOnlyDictionary<Key, Value> AddOnlyDictionary<Key, Value>(bool CanModify = false, IDictionary<Key, Value>? Dictionary = null)
            where Key : notnull
            => new AddOnlyDictionary<Key, Value>(Dictionary, CanModify);
        #endregion
        #region 创建线程安全字典
        /// <summary>
        /// 创建一个线程安全，且只能添加元素的字典，并返回
        /// </summary>
        /// <typeparam name="Key">字典的键类型</typeparam>
        /// <typeparam name="Value">字典的值类型</typeparam>
        ///  <param name="CanModify">如果这个值为<see langword="true"/>，
        /// 代表可以修改已经被添加到字典的值，否则代表禁止修改</param>
        /// <returns></returns>
        public static IAddOnlyDictionary<Key, Value> AddOnlyDictionaryConcurrent<Key, Value>(bool CanModify = false)
            where Key : notnull
            => AddOnlyDictionary(CanModify, new ConcurrentDictionary<Key, Value>());
        #endregion
        #endregion
    }
}
