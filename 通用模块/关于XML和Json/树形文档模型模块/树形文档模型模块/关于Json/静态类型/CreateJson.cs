using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace System.TreeObject.Json
{
    /// <summary>
    /// 这个静态类被用来创建有关Json的对象
    /// </summary>
    public static class CreateJson
    {
        #region 返回序列化类型非法所引起的异常
        /// <summary>
        /// 返回由于无法序列化或反序列化类型所引发的异常
        /// </summary>
        /// <param name="type">引发异常的非法类型</param>
        /// <returns></returns>
        public static JsonException ExceptionType(Type type)
            => new($"无法序列化或反序列化类型{type}");
        #endregion
        #region 通过ISerialization创建JsonSerializerOptions
        /// <summary>
        /// 通过<see cref="ISerialization{Output}"/>创建<see cref="JsonSerializerOptions"/>
        /// </summary>
        /// <typeparam name="Output">序列化器的输出对象类型</typeparam>
        /// <param name="serialization">用来序列化或反序列化的对象</param>
        /// <returns>如果<paramref name="serialization"/>为<see langword="null"/>，
        /// 则返回<see langword="null"/>，否则返回一个利用<paramref name="serialization"/>进行序列化的<see cref="JsonSerializerOptions"/></returns>
        [return: NotNullIfNotNull("serialization")]
        public static JsonSerializerOptions? SerializerOptions<Output>(ISerialization<Output>? serialization)
        {
            switch (serialization)
            {
                case null:
                    return null;
                case { Agreement: string a } s when !a.EqualsIgnore("json"):
                    throw new NotSupportedException($"{s}支持的协议为{a}，不支持Json");
                case { } s:
                    var op = new JsonSerializerOptions();
                    op.Converters.Add(s.FitJsonConverter());
                    return op;
            }
        }
        #endregion
    }
}
