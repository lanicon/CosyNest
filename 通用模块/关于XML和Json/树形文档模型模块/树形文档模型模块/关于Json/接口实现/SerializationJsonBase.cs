using System.Text;
using System.Text.Json;

namespace System.TreeObject.Json
{
    /// <summary>
    /// <see cref="ISerialization{Output}"/>的可选基类，
    /// 它假设派生类只能序列化Json，且底层使用<see cref="JsonSerializerOptions"/>来实现
    /// </summary>
    /// <typeparam name="Input">支持进行序列化的输入类型</typeparam>
    /// <typeparam name="Output">支持进行反序列化的输出类型</typeparam>
    public abstract class SerializationJsonBase<Input, Output> : ISerialization<Output>
    {
        #region 返回协议名称
        public string Agreement
             => "Json";
        #endregion
        #region 关于序列化
        #region 检查是否可序列化
        public bool CanSerialization(Type type)
            => typeof(Input).IsAssignableFrom(type);
        #endregion
        #region 序列化为指定编码
        public ReadOnlySpan<byte> Serialization(object? obj, Encoding? encoding = null)
             => obj is { } && !CanSerialization(obj.GetType()) ?             //null值无需检查类型，可以被直接序列化
             throw CreateJson.ExceptionType(obj.GetType()) :
             SerializationTemplate(obj, encoding);
        #endregion
        #region 模板方法
        /// <summary>
        /// 将受支持的对象序列化为指定编码的模板方法，
        /// 它无需检查<paramref name="obj"/>的类型
        /// </summary>
        /// <param name="obj">待序列化的对象</param>
        /// <param name="encoding">序列化的目标编码，
        /// 如果为<see langword="null"/>，默认为UTF8</param>
        /// <returns></returns>
        protected abstract ReadOnlySpan<byte> SerializationTemplate(object? obj, Encoding? encoding = null);
        #endregion
        #region 辅助方法
        /// <summary>
        /// 序列化的辅助方法，根据输出编码的不同，
        /// 它尝试调用<see cref="JsonSerializer.SerializeToUtf8Bytes(object, Type, JsonSerializerOptions)"/>或
        /// <see cref="JsonSerializer.SerializeToUtf8Bytes(object, Type, JsonSerializerOptions)"/>进行序列化
        /// </summary>
        /// <param name="obj">待序列化的对象</param>
        /// <param name="encoding">序列化的目标编码，
        /// 如果为<see langword="null"/>，默认为UTF8</param>
        /// <param name="targetType">输出的目标类型，如果为<see langword="null"/>，
        /// 则调用<see cref="object.GetType"/>方法获取<paramref name="obj"/>的类型</param> 
        /// <param name="options">用于序列化的选项</param>
        /// <returns></returns>
        protected ReadOnlySpan<byte> SerializationAided(object? obj, Encoding? encoding, Type? targetType = null, JsonSerializerOptions? options = null)
        {
            targetType ??= obj?.GetType() ?? typeof(object);
            return encoding is null or UTF8Encoding ?
                    JsonSerializer.SerializeToUtf8Bytes(obj, targetType, options) :
                    encoding.GetBytes(JsonSerializer.Serialize(obj, targetType, options));
        }
        #endregion
        #endregion
        #region 关于反序列化
        #region 从指定的编码反序列化
        public abstract Output? Deserialize(ReadOnlySpan<byte> text, Encoding? encoding = null);
        #endregion
        #region 辅助方法
        /// <summary>
        /// 反序列化的辅助方法，根据目标编码的不同，
        /// 它尝试调用<see cref="JsonSerializer.Deserialize{TValue}(string, JsonSerializerOptions?)"/>或
        /// <see cref="JsonSerializer.Deserialize{TValue}(ReadOnlySpan{byte}, JsonSerializerOptions?)"/>进行反序列化
        /// </summary>
        /// <typeparam name="Obj">反序列化的目标类型</typeparam>
        /// <param name="text">用来描述对象的文本</param>
        /// <param name="encoding">文本的编码，
        /// 如果为<see langword="null"/>，默认为UTF8</param>
        /// <param name="options">用于序列化的选项</param>
        /// <returns></returns>
        protected Obj? DeserializeAided<Obj>(ReadOnlySpan<byte> text, Encoding? encoding = null, JsonSerializerOptions? options = null)
            => encoding is null or UTF8Encoding ?
                JsonSerializer.Deserialize<Obj>(text, options) :
                JsonSerializer.Deserialize<Obj>(encoding.GetString(text), options);
        #endregion
        #endregion
    }
}
