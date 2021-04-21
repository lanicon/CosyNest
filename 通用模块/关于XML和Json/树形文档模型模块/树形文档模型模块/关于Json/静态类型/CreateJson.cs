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
    }
}
