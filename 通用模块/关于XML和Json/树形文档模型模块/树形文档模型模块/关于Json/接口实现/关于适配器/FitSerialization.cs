using System.Linq;
using System.Text;
using System.Text.Json;

namespace System.TreeObject.Json
{
    /// <summary>
    /// 该类型可以用来适配<see cref="ISerialization{Output}"/>
    /// </summary>
    /// <typeparam name="Output"></typeparam>
    class FitSerialization<Output> : SerializationBase<Output>
    {
        #region 封装的Json转换器
        /// <summary>
        /// 获取封装的Json转换器对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private ISerialization<Output> Converter { get; }
        #endregion
        #region 序列化对象
        #region JsonConverter版本
        public override void Write(Utf8JsonWriter writer, Output value, JsonSerializerOptions options)
            => writer.WriteBase64StringValue(Converter.Serialization(value));
        #endregion
        #region ISerialization版本
        public override ReadOnlySpan<byte> Serialization(object? obj, Encoding? encoding = null)
            => Converter.Serialization(obj, encoding);
        #endregion
        #endregion 
        #region 反序列化对象
        #region JsonConverter版本
        public override Output? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => Converter.Deserialize(reader.ReadAll().ToArray());
        #endregion
        #region ISerialization版本
        public override Output? Deserialize(ReadOnlySpan<byte> text, Encoding? encoding = null)
            => Converter.Deserialize(text, encoding);
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的Json转换器对象初始化对象
        /// </summary>
        /// <param name="converter">Json转换器对象，本对象的功能就是通过它实现的</param>
        public FitSerialization(ISerialization<Output> converter)
        {
            this.Converter = converter;
        }
        #endregion
    }
}
