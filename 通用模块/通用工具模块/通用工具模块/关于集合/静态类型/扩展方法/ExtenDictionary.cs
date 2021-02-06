using System.Collections.Generic;

namespace System.Linq
{
    public static partial class ExtenIEnumerable
    {
        /*所有有关字典的扩展方法，全部放在这个部分类中*/

        #region 关于索引器
        #region 安全索引
        #region 如果不存在键，获取值并将其添加到字典中
        /// <summary>
        /// 根据键在字典中查找值，如果键不存在，
        /// 则通过委托获取一个值，并将其添加到字典中
        /// </summary>
        /// <typeparam name="Key">字典的键类型</typeparam>
        /// <typeparam name="Value">字典的值类型</typeparam>
        /// <param name="Dict">需要进行判断的字典</param>
        /// <param name="key">指定的键</param>
        /// <param name="NewValue">如果键不存在，则通过这个委托获取值，并将其添加到字典中，委托的参数就是刚才的键</param>
        /// <returns>返回值是一个元组，第一个项是键是否存在，第二个项是值</returns>
        public static (bool Exist, Value Value) TrySetValue<Key, Value>(this IDictionary<Key, Value> Dict, Key key, Func<Key, Value> NewValue)
            where Key : notnull
        {
            if (Dict.TryGetValue(key, out var values))
                return (true, values);
            var New = NewValue(key);
            Dict.Add(key, New);
            return (false, New);
        }
        #endregion
        #region 如果不存在键，返回指定值
        /// <summary>
        /// 根据键在字典中查找值，并返回一个元组，
        /// 第一个元素指示键是否存在，如果存在，第二个元素是找到的值，
        /// 如果不存在，第二个元素是一个指定的默认值
        /// </summary>
        /// <typeparam name="Key">字典的键类型</typeparam>
        /// <typeparam name="Value">字典的值类型</typeparam>
        /// <param name="Dict">这个参数必须是一个只读字典或可变字典</param>
        /// <param name="key">用来提取值的键，如果它为<see langword="null"/>，则默认该键不存在</param>
        /// <param name="NoFound">如果键不存在，则通过这个延迟对象返回默认值</param>
        /// <returns></returns>
        public static (bool Exist, Value? Value) TryGetValue<Key, Value>(this IEnumerable<KeyValuePair<Key, Value>> Dict, Key key, LazyPro<Value>? NoFound = default)
        {
            if (Dict is IDictionary<Key, Value> or IReadOnlyDictionary<Key, Value>)
            {
                return Dict.To<dynamic>().TryGetValue(key, out Value value) ?
                 (true, value) : (false, (Value)NoFound);
            }
            throw new ExceptionTypeUnlawful(Dict, typeof(IDictionary<Key, Value>), typeof(IReadOnlyDictionary<Key, Value>));
        }
        #endregion
        #endregion
        #endregion
        #region 关于字典转换
        #region 将字典转换为其他字典
        /// <summary>
        /// 将一个字典转换为其他类型字典
        /// </summary>
        /// <typeparam name="Ret">转换的目标类型</typeparam>
        /// <typeparam name="Key">字典的键类型</typeparam>
        /// <typeparam name="Value">字典的值类型</typeparam>
        /// <param name="Dict">要转换的字典</param>
        /// <returns></returns>
        public static Ret ToDictionary<Ret, Key, Value>(this IDictionary<Key, Value> Dict)
            where Key : notnull
            where Ret : IDictionary<Key, Value>, new()
        {
            var New = new Ret();
            Dict.ForEach(New.Add);
            return New;
        }
        #endregion
        #region 将集合转换为字典
        #region 将任意集合转换为字典
        /// <summary>
        /// 从集合中提取键和值，并将集合转换为字典
        /// </summary>
        /// <typeparam name="Key">字典的键类型</typeparam>
        /// <typeparam name="Value">字典的值类型</typeparam>
        /// <typeparam name="Obj">集合的元素类型</typeparam>
        /// <param name="List">待转换的集合</param>
        /// <param name="Del">这个委托传入集合的元素，并返回一个元组，它的项分别是字典的键和值</param>
        /// <param name="CheckRepetition">如果这个值为<see langword="true"/>，在向字典添加重复键的时候会报错，否则会覆盖旧有键的值</param>
        /// <returns></returns>
        public static Dictionary<Key, Value> ToDictionary<Key, Value, Obj>(this IEnumerable<Obj> List, Func<Obj, (Key, Value)> Del, bool CheckRepetition)
            where Key : notnull
        {
            var dict = new Dictionary<Key, Value>();
            foreach (var item in List)
            {
                var (K, V) = Del(item);
                if (CheckRepetition)
                    dict.Add(K, V);
                else dict[K] = V;
            }
            return dict;
        }
        #endregion
        #region 将键值对集合直接转换为字典
        /// <summary>
        /// 将键值对集合直接转换为字典
        /// </summary>
        /// <typeparam name="Key">键的类型</typeparam>
        /// <typeparam name="Value">值的类型</typeparam>
        /// <param name="List">要转换为字典的键值对集合</param>
        /// <param name="CheckRepetition">如果这个值为<see langword="true"/>，在向字典添加重复键的时候会报错，否则会覆盖旧有键的值</param>
        /// <returns></returns>
        public static Dictionary<Key, Value> ToDictionary<Key, Value>(this IEnumerable<KeyValuePair<Key, Value>> List, bool CheckRepetition)
            where Key : notnull
            => List.ToDictionary(x => (x.Key, x.Value), CheckRepetition);
        #endregion
        #region 将元组集合直接转换为字典
        /// <summary>
        /// 将一个元组集合直接转换为字典
        /// </summary>
        /// <typeparam name="Key">字典的键类型</typeparam>
        /// <typeparam name="Value">字典的值类型</typeparam>
        /// <param name="List">待转换的元组集合</param>
        /// <param name="CheckRepetition">如果这个值为<see langword="true"/>，在向字典添加重复键的时候会报错，否则会覆盖旧有键的值</param>
        /// <returns></returns>
        public static Dictionary<Key, Value> ToDictionary<Key, Value>(this IEnumerable<(Key, Value)> List, bool CheckRepetition)
            where Key : notnull
            => List.ToDictionary(x => (x.Item1, x.Item2), CheckRepetition);
        #endregion
        #endregion
        #endregion
        #region 关于键值对
        #region 将元组转换为键值对
        /// <summary>
        /// 将一个二元组转换为键值对
        /// </summary>
        /// <typeparam name="Key">键值对的键类型</typeparam>
        /// <typeparam name="Value">键值对的值类型</typeparam>
        /// <param name="Tupts">待转换的元组</param>
        /// <returns></returns>
        public static KeyValuePair<Key, Value> ToKV<Key, Value>(this (Key, Value) Tupts)
            where Key : notnull
            => new KeyValuePair<Key, Value>(Tupts.Item1, Tupts.Item2);
        #endregion
        #region 批量转换元组为键值对
        /// <summary>
        /// 将二元组集合转换为键值对集合
        /// </summary>
        /// <typeparam name="Key">键值对的键类型</typeparam>
        /// <typeparam name="Value">键值对的值类型</typeparam>
        /// <param name="List">待转换的二元组集合</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<Key, Value>> ToKV<Key, Value>(this IEnumerable<(Key, Value)> List)
            where Key : notnull
            => List.Select(ToKV);
        #endregion
        #region 批量解构键值对
        /// <summary>
        /// 批量将键值对解构为元组
        /// </summary>
        /// <typeparam name="Key">键值对的键类型</typeparam>
        /// <typeparam name="Value">键值对的值类型</typeparam>
        /// <param name="List">要解构的键值对集合</param>
        /// <returns></returns>
        public static IEnumerable<(Key key, Value value)> ToTupts<Key, Value>(this IEnumerable<KeyValuePair<Key, Value>> List)
            => List.Select(x => (x.Key, x.Value));
        #endregion
        #endregion
    }
}
