using System.Collections.Generic;
using System.DataFrancis;
using System.Design.Direct;
using System.Linq;
using System.TreeObject.Json;

namespace System.Text.Json.Serialization
{
    /// <summary>
    /// 这个类型可以使用Json序列化和反序列化<see cref="IDirect"/>
    /// </summary>
    public sealed class JsonConverterIDirect : SerializationBase<IDirect>
    {
        #region 返回是否可序列化类型
        public override bool CanConvert(Type typeToConvert)
            => typeof(IDirect).IsAssignableFrom(typeToConvert);
        #endregion
        #region 反序列化
        #region 说明文档
        /*对IDirect的反序列化遵循以下原则：
          #对于数字类型，反序列化为System.Num
          #对于布尔类型，反序列化为Boolean
          #对于字符串，如果它可以被解释为DateTime，TimeOffset或Guid，
          则将其反序列化为对应类型，否则反序列化为String
          #对于集合，一律反序列化为数组
          #对于除此以外的复杂对象，一律反序列化为IDirect，
          它的键值对对应对象的属性和值*/
        #endregion
        #region 辅助方法
        #region 反序列化为IDirect
        /// <summary>
        /// 读取<see cref="Utf8JsonReader"/>的对象部分，
        /// 并将其反序列化为<see cref="IDirect"/>
        /// </summary>
        /// <param name="reader">待读取的<see cref="Utf8JsonReader"/></param>
        /// <returns></returns>
        private IDirect ReadDirect(ref Utf8JsonReader reader)
        {
            var link = new LinkedList<(string, object?)>();
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.None or JsonTokenType.Comment or JsonTokenType.StartObject:
                        break;
                    case JsonTokenType.EndObject:
                        return CreateDataObj.Data(link.ToArray());
                    case JsonTokenType.PropertyName:
                        link.AddLast((reader.GetString()!, ReadObject(ref reader))); break;
                    case var type:
                        throw new JsonException($"未识别{type}类型的令牌");
                }
            }
            throw new JsonException($"读取器已到达末尾，但是没有找到{JsonTokenType.EndObject}令牌");
        }

        /*问：JsonTokenType.StartObject和JsonTokenType.EndObject必然成对出现，
          在合法的JSON字符串中，只需要检查是否到达JsonTokenType.EndObject即可，
          既然如此，为什么需要检查是否到达读取器的末尾？
          答：在合法的JSON字符串中确实如此，但是在实际运行中，有可能传入不合法的JSON字符串，
          如果不检查是否到达读取器的末尾，而且此时有人恶意传入不带EndObject令牌的字符串，
          在这种情况下，函数会陷入无限循环且不会抛出异常*/
        #endregion
        #region 反序列化为任何对象
        /// <summary>
        /// 读取<see cref="Utf8JsonReader"/>的一部分，
        /// 并根据它们的格式将其反序列化为基本类型，数组，或<see cref="IDirect"/>
        /// </summary>
        /// <param name="reader">待读取的<see cref="Utf8JsonReader"/></param>
        /// <returns></returns>
        private object? ReadObject(ref Utf8JsonReader reader)
        {
            if (!reader.Read())
                throw new JsonException("读取器已到达末尾");
            var (IsSuccess, Value) = reader.TryGetBasicType();
            return IsSuccess ? Value : reader.TokenType switch
            {
                JsonTokenType.StartObject => ReadDirect(ref reader),
                JsonTokenType.StartArray => ReadArray(ref reader),
                JsonTokenType.EndArray => null,
                JsonTokenType.Comment or JsonTokenType.None => ReadObject(ref reader),
                var type => throw new JsonException($"未识别{type}类型的令牌")
            };
        }
        #endregion
        #region 反序列化为数组
        /// <summary>
        /// 读取<see cref="Utf8JsonReader"/>的数组部分，
        /// 并将其反序列化为数组
        /// </summary>
        /// <param name="reader">待读取的<see cref="Utf8JsonReader"/></param>
        /// <returns></returns>
        private object?[] ReadArray(ref Utf8JsonReader reader)
        {
            var link = new LinkedList<object?>();
            while (true)
            {
                var elements = ReadObject(ref reader);
                if (reader.TokenType is JsonTokenType.EndArray)
                    return link.ToArray();
                link.AddLast(elements);
            }
        }
        #endregion
        #endregion
        #region 正式方法
        public override IDirect Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => ReadDirect(ref reader);
        #endregion
        #endregion
        #region 序列化
        public override void Write(Utf8JsonWriter writer, IDirect value, JsonSerializerOptions options)
        {
            var self = options.Converters.PackIndex().Where(x => x.Elements is JsonConverterIDirect).ToArray();     //避免无限循环Bug
            if (self.Any())
            {
                options = new(options);
                self.ForEach(x => options.Converters.RemoveAt(x.Index));
            }
            JsonSerializer.Serialize(writer, value, options);
        }
        #endregion
    }
}
