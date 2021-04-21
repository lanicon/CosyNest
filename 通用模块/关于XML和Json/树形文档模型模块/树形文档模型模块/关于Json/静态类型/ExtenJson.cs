using System.Buffers;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.TreeObject;
using System.TreeObject.Json;

namespace System
{
    /// <summary>
    /// 有关JSON的扩展方法全部放在这里
    /// </summary>
    public static class ExtenJson
    {
        #region 通过JsonConverter创建JsonSerializerOptions
        /// <summary>
        /// 创建一个<see cref="JsonSerializerOptions"/>，
        /// 并将一个<see cref="JsonConverter"/>添加到它的转换器列表中
        /// </summary>
        /// <param name="converter">待添加进转换器列表的<see cref="JsonConverter"/></param>
        /// <returns></returns>
        public static JsonSerializerOptions ToOptions(this JsonConverter converter)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(converter);
            return options;
        }
        #endregion
        #region 关于适配
        #region 从JsonConverter<T>适配
        /// <summary>
        /// 将<see cref="JsonConverter{T}"/>适配为<see cref="SerializationBase{Output}"/>
        /// </summary>
        /// <typeparam name="Output">可序列化的目标类型</typeparam>
        /// <param name="converter">待适配的转换器</param>
        /// <returns></returns>
        public static SerializationBase<Output> Fit<Output>(this JsonConverter<Output> converter)
            => converter is SerializationBase<Output> c ? c : new FitJsonConverter<Output>(converter);
        #endregion
        #region 从ISerialization<T>适配
        /// <summary>
        /// 将<see cref="ISerialization{Output}"/>适配为<see cref="SerializationBase{Output}"/>
        /// </summary>
        /// <typeparam name="Output">可序列化的目标类型</typeparam>
        /// <param name="serialization">待适配的序列化器</param>
        /// <returns></returns>
        public static SerializationBase<Output> Fit<Output>(this ISerialization<Output> serialization)
            => serialization is SerializationBase<Output> s ? s : new FitSerialization<Output>(serialization);
        #endregion
        #endregion
        #region 关于读取Json
        #region 读取Json的全部内容
        /// <summary>
        /// 读取一个<see cref="Utf8JsonReader"/>的全部内容，
        /// 警告：调用这个方法会改变该对象的状态
        /// </summary>
        /// <param name="reader">待读取内容的<see cref="Utf8JsonReader"/></param>
        /// <returns></returns>
        public static IEnumerable<byte> ReadAll(this ref Utf8JsonReader reader)
        {
            var list = new List<byte>();
            while (reader.Read())
            {
                list.AddRange(reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan.ToArray());
            }
            return list;
        }

        /*问：list仅需要添加和枚举元素的功能，
          为什么不使用时间复杂度为O(1)的LinkedList<byte>来替代它？
          答：因为选择数据结构需要考虑到实际情况，
          LinkedList<byte>需要为每个元素维护一个LinkedListNode<byte>，
          但是byte只需要占用一个字节的内存，为了它创建一个这么庞大的对象是不划算的，
          而List<byte>在内部使用数组，没有上述的额外开销，而且List.AddRange方法为数组特别优化过，
          综上所述，在这种情况下，使用时间复杂度最高为O(n)的List较O(1)的的LinkedList性能更高*/
        #endregion
        #region 反序列化为基本类型
        /// <summary>
        /// 读取<see cref="Utf8JsonReader"/>的令牌，并尝试将其解释为基本类型
        /// </summary>
        /// <param name="reader">待读取令牌的<see cref="Utf8JsonReader"/></param>
        /// <returns>一个元组，它的项分别是是否成功解释为基本类型，以及转换后的基本类型的值，
        /// 本函数可以处理的基本类型包括：<see cref="bool"/>，<see cref="Num"/>（以及可转换为它的数字类型），
        /// <see cref="DateTime"/>，<see cref="DateTimeOffset"/>，<see cref="Guid"/>，
        /// <see cref="string"/>和<see langword="null"/>值</returns>
        public static (bool IsSuccess, object? Value) TryGetBasicType(this in Utf8JsonReader reader)
            => reader.TokenType switch
            {
                JsonTokenType.True or JsonTokenType.False => (true, reader.GetBoolean()),
                JsonTokenType.Null => (true, null),
                JsonTokenType.Number => (true, (Num)reader.GetDecimal()),
                JsonTokenType.String => reader switch
                {
                    var r when r.TryGetDateTimeOffset(out var value) => (true, value),
                    var r when r.TryGetDateTime(out var value) => (true, value),
                    var r when r.TryGetGuid(out var value) => (true, value),
                    var r => (true, r.GetString())
                },
                _ => (false, null)
            };
        #endregion
        #endregion 
        #region 关于写入Json
        #region 序列化基本类型
        /// <summary>
        /// 尝试将基本类型写入<see cref="Utf8JsonWriter"/>
        /// </summary>
        /// <param name="writer">待写入的<see cref="Utf8JsonWriter"/>对象</param>
        /// <param name="propertyName">属性的名称，如果这个值为<see langword="null"/>，
        /// 表示将<paramref name="value"/>作为数组的元素写入，否则表示将其作为属性和属性的值写入</param>
        /// <param name="value">要写入的值</param>
        /// <returns>如果<paramref name="value"/>是基本类型，
        /// 则将其写入，并返回<see langword="true"/>，否则不执行其他操作，并返回<see langword="false"/>，
        /// 基本类型包括：<see cref="bool"/>，<see cref="Num"/>（以及可转换为它的数字类型），
        /// <see cref="DateTime"/>，<see cref="DateTimeOffset"/>，<see cref="Guid"/>，
        /// <see cref="string"/>，<see cref="Enum"/>和<see langword="null"/>值</returns>
        public static bool TryWriteBasicType(this Utf8JsonWriter writer, string? propertyName, object? value)
        {
            switch (value)
            {
                case null:
                    writer.TryWritePropertyName(propertyName);
                    writer.WriteNullValue();
                    return true;
                case string o:
                    writer.TryWritePropertyName(propertyName);
                    writer.WriteStringValue(o);
                    return true;
                case bool o:
                    writer.TryWritePropertyName(propertyName);
                    writer.WriteBooleanValue(o);
                    return true;
                case Enum o:
                    writer.TryWritePropertyName(propertyName);
                    writer.WriteStringValue(o.ToString());
                    return true;
                case Num o:
                    writer.TryWritePropertyName(propertyName);
                    writer.WriteNumberValue(o.Value);
                    return true;
                case int o:
                    writer.TryWritePropertyName(propertyName);
                    writer.WriteNumberValue(o);
                    return true;
                case double o:
                    writer.TryWritePropertyName(propertyName);
                    writer.WriteNumberValue(o);
                    return true;
                case long o:
                    writer.TryWritePropertyName(propertyName);
                    writer.WriteNumberValue(o);
                    return true;
                case decimal o:
                    writer.TryWritePropertyName(propertyName);
                    writer.WriteNumberValue(o);
                    return true;
                case float o:
                    writer.TryWritePropertyName(propertyName);
                    writer.WriteNumberValue(o);
                    return true;
                case uint o:
                    writer.TryWritePropertyName(propertyName);
                    writer.WriteNumberValue(o);
                    return true;
                case ulong o:
                    writer.TryWritePropertyName(propertyName);
                    writer.WriteNumberValue(o);
                    return true;
                case DateTime o:
                    writer.TryWritePropertyName(propertyName);
                    writer.WriteStringValue(o);
                    return true;
                case DateTimeOffset o:
                    writer.TryWritePropertyName(propertyName);
                    writer.WriteStringValue(o);
                    return true;
                case Guid o:
                    writer.TryWritePropertyName(propertyName);
                    writer.WriteStringValue(o);
                    return true;
                default:
                    return false;
            }
        }
        #endregion
        #region 尝试写入属性的名称
        /// <summary>
        /// 如果<paramref name="propertyName"/>不为<see langword="null"/>，
        /// 则向<paramref name="writer"/>中写入属性的名称，否则不执行任何操作
        /// </summary>
        /// <param name="writer">待写入属性名称的<see cref="Utf8JsonWriter"/></param>
        /// <param name="propertyName">待写入的属性名称，
        /// 如果为<see langword="null"/>，则不执行任何操作</param>
        public static void TryWritePropertyName(this Utf8JsonWriter writer, string? propertyName)
        {
            if (propertyName is not null)
                writer.WritePropertyName(propertyName);
        }
        #endregion
        #endregion 
    }
}
