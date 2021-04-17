using System.Net.Http;
using System.Net.Http.Headers;
using System.NetFrancis.Http;
using System.Linq;

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
    }
}
