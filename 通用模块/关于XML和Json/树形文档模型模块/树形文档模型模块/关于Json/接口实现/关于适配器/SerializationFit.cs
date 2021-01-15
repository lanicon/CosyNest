using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System.TreeObject.Json
{
    /// <summary>
    /// 这个类型实现了<see cref="ISerialization{Output}"/>，
    /// 可以用来适配<see cref="JsonConverter"/>
    /// </summary>
    /// <typeparam name="Ret">可序列化的目标类型</typeparam>
    class SerializationFit<Ret> : SerializationJsonBase<Ret, Ret>
    {
        #region 封装的对象
        #region Json转换器
        /// <summary>
        /// 获取封装的Json转换器对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        internal JsonConverter<Ret> Converter { get; }
        #endregion
        #region 序列化选项
        /// <summary>
        /// 获取用于序列化Json的选项
        /// </summary>
        private JsonSerializerOptions Options { get; }
        #endregion
        #endregion
        #region 序列化对象
        protected override ReadOnlySpan<byte> SerializationTemplate(object? obj, Encoding? encoding = null)
            => SerializationAided(obj, encoding, typeof(Ret), Options);
        #endregion
        #region 反序列化对象
        public override Ret? Deserialize(ReadOnlySpan<byte> text, Encoding? encoding = null)
            => DeserializeAided<Ret>(text, encoding, Options);
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的转换器初始化对象
        /// </summary>
        /// <param name="Converter">Json转换器对象，本对象的功能就是通过它实现的</param>
        public SerializationFit(JsonConverter Converter)
        {
            this.Converter = Converter.GetConverter<Ret>();
            var op = new JsonSerializerOptions();
            op.Converters.Add(this.Converter);
            Options = op;
        }
        #endregion
    }
}
