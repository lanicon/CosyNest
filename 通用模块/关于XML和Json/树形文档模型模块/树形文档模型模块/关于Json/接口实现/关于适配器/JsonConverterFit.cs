using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System.TreeObject.Json
{
    /// <summary>
    /// 这个类型继承自<see cref="JsonConverter{T}"/>，
    /// 可以用来适配<see cref="ISerialization{Output}"/>
    /// </summary>
    /// <typeparam name="Obj">序列化和反序列化的合法类型</typeparam>
    class JsonConverterFit<Obj> : JsonConverter<Obj>
    {
        #region 封装的序列化对象
        /// <summary>
        /// 获取封装的序列化对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        internal ISerialization<Obj> Serialization { get; }
        #endregion
        #region 反序列化
        public override Obj? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => Serialization.Deserialize(reader.ReadAll().ToArray());
        #endregion
        #region 序列化
        public override void Write(Utf8JsonWriter writer, Obj value, JsonSerializerOptions options)
            => writer.WriteBase64StringValue(Serialization.Serialization(value));
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的序列化对象初始化对象
        /// </summary>
        /// <param name="Serialization">指定的序列化对象，
        /// 本对象的功能就是通过它实现的</param>
        public JsonConverterFit(ISerialization<Obj> Serialization)
        {
            if (!Serialization.Agreement.EqualsIgnore("json"))
                throw new ArgumentException($"该{typeof(ISerialization<Obj>)}所支持的序列化协议为{Serialization.Agreement}，它不支持Json");
            this.Serialization = Serialization;
        }
        #endregion
    }
}
