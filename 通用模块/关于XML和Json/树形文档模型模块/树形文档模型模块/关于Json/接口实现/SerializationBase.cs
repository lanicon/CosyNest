using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System.TreeObject.Json
{
    /// <summary>
    /// 这个类型是实现<see cref="ISerialization{Output}"/>的可选基类，
    /// 它还可以同时兼容于<see cref="JsonConverter{T}"/>
    /// </summary>
    /// <typeparam name="Output">反序列化的合法输出类型</typeparam>
    public abstract class SerializationBase<Output> : JsonConverter<Output>, ISerialization<Output>
    {
        #region 返回协议名称
        public string Agreement
             => ISerialization.Json;
        #endregion
        #region 关于转换编码
        #region 转换输出编码
        /// <summary>
        /// 将输出编码转换为指定的编码并返回
        /// </summary>
        /// <param name="output">输出编码</param>
        /// <param name="encoding">要返回的字节跨度的编码，
        /// 如果为<see langword="null"/>，则默认为UTF8</param>
        /// <returns></returns>
        protected static ReadOnlySpan<byte> ConvertOutput(in ReadOnlySpan<byte> output, Encoding? encoding)
            => encoding is null or UTF8Encoding ?
            output :
            Encoding.Convert(Encoding.UTF8, encoding, output.ToArray());
        #endregion
        #region 转换输入编码
        /// <summary>
        /// 将输入编码转换为UTF8并返回
        /// </summary>
        /// <param name="input">输入编码</param>
        /// <param name="encoding">输入编码的原始编码类型，
        /// 如果为<see langword="null"/>，则默认为UTF8</param>
        /// <returns></returns>
        protected static ReadOnlySpan<byte> ConvertInput(in ReadOnlySpan<byte> input, Encoding? encoding)
            => encoding is null or UTF8Encoding ?
            input :
            Encoding.Convert(encoding, Encoding.UTF8, input.ToArray());
        #endregion
        #endregion
        #region 关于序列化
        #region 检查是否可序列化
        public bool CanSerialization(Type type)
            => typeof(Output).IsAssignableFrom(type);
        #endregion
        #region 序列化对象
        public virtual ReadOnlySpan<byte> Serialization(object? obj, Encoding? encoding = null)
        {
            var text = JsonSerializer.SerializeToUtf8Bytes(obj, typeof(Output), this.ToOptions());
            return ConvertOutput(text, encoding);
        }
        #endregion
        #endregion
        #region 反序列化对象
        public virtual Output? Deserialize(ReadOnlySpan<byte> text, Encoding? encoding = null)
            => JsonSerializer.Deserialize<Output>(ConvertInput(text, encoding), this.ToOptions());
        #endregion
    }
}
