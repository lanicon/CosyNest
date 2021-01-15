using System.Net.Http;
using System.Net.Http.Headers;
using System.NetFrancis.Http;
using System.Text;
using System.Text.Json;
using System.TreeObject;

namespace System
{
    /// <summary>
    /// 有关网络的扩展方法全部放在这里
    /// </summary>
    public static class ExtenNet
    {
        #region 有关实现IHttpClient
        #region 将HttpRequestRecording转换为HttpRequestMessage
        /// <summary>
        /// 将<see cref="HttpRequestRecording"/>转换为等效的<see cref="HttpRequestMessage"/>
        /// </summary>
        /// <param name="recording">待转换的<see cref="HttpRequestRecording"/></param>
        /// <returns></returns>
        internal static HttpRequestMessage ToHttpRequestMessage(this IHttpRequestRecording recording)
        {
            var m = new HttpRequestMessage()
            {
                RequestUri = new(recording.UriComplete),
                Method = recording.HttpMethod,
                Content = recording.Content.ToHttpContent()
            };
            recording.Header.CopyHeader(m.Headers);
            return m;
        }
        #endregion
        #region 将IHttpContent转换为HttpContent
        /// <summary>
        /// 将<see cref="IHttpContent"/>转换为<see cref="HttpContent"/>
        /// </summary>
        /// <param name="content">待转换的<see cref="HttpContent"/></param>
        /// <returns></returns>
        private static HttpContent? ToHttpContent(this IHttpContent? content)
        {
            if (content is null)
                return null;
            var arryContent = new ByteArrayContent(content.Content.ToArray());
            content.Header.CopyHeader(arryContent.Headers);
            return arryContent;
        }
        #endregion
        #region 将IHttpHeader的标头复制到HttpHeaders
        /// <summary>
        /// 将<see cref="IHttpHeader"/>的所有标头复制到另一个<see cref="HttpHeaders"/>中
        /// </summary>
        /// <param name="header">待复制标头的<see cref="IHttpHeader"/></param>
        /// <param name="bclHeader"><paramref name="header"/>的所有标头将被复制到这个参数中</param>
        private static void CopyHeader(this IHttpHeader header, HttpHeaders bclHeader)
        {
            foreach (var (key, value) in header.Headers())
            {
                bclHeader.Add(key, value);
            }
        }
        #endregion
        #endregion
        #region 有关IHttpContent
        #region 说明文档
        /*重要说明：
          由于时间限制，这些API暂时没有考虑到Http正文使用非UTF8编码的情况，
          如果以后时间充足或因此出现了问题，请将其重构*/
        #endregion
        #region 解释为文本
        /// <summary>
        /// 将一个<see cref="IHttpContent"/>的内容解释为文本，并返回
        /// </summary>
        /// <param name="content">待解释的<see cref="IHttpContent"/></param>
        /// <returns></returns>
        public static string ToText(this IHttpContent content)
            => Encoding.UTF8.GetString(content.Content);
        #endregion
        #region 解释为Json
        /// <summary>
        /// 将<see cref="IHttpContent"/>正文解释为Json，
        /// 并将其反序列化返回
        /// </summary>
        /// <typeparam name="Obj">反序列化的返回类型</typeparam>
        /// <param name="content">待反序列化的<see cref="IHttpContent"/>对象</param>
        /// <param name="options">用于配置反序列化的选项</param>
        /// <returns></returns>
        public static Obj? ToJson<Obj>(this IHttpContent content, JsonSerializerOptions? options = null)
            => JsonSerializer.Deserialize<Obj>(content.Content, options);
        #endregion
        #region 解释为树形文档对象
        /// <summary>
        /// 将<see cref="IHttpContent"/>正文解释为树形文档对象，
        /// 并将其反序列化返回
        /// </summary>
        /// <typeparam name="Obj">反序列化的返回类型</typeparam>
        /// <param name="content">待反序列化的<see cref="IHttpContent"/>对象</param>
        /// <param name="serialization">用来反序列化<paramref name="content"/>的对象</param>
        /// <returns></returns>
        public static Obj? ToTreeObject<Obj>(this IHttpContent content, ISerialization<Obj> serialization)
            => serialization.Deserialize(content.Content);
        #endregion
        #endregion
    }
}
