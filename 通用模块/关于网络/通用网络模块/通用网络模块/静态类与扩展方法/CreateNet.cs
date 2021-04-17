using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.NetFrancis.Http;
using System.Text.Json;

namespace System.NetFrancis
{
    /// <summary>
    /// 这个静态类可以用来创建和网络相关的对象
    /// </summary>
    public static class CreateNet
    {
        #region 返回公用的IHttpClient对象
        /// <summary>
        /// 返回一个公用的<see cref="IHttpClient"/>对象
        /// </summary>
        public static IHttpClient HttpClient { get; }
        = new HttpClientRealize();
        #endregion
        #region 有关IHttpContent
        #region 使用Json创建
        /// <summary>
        /// 将指定对象序列化，
        /// 然后创建一个包含Json的<see cref="HttpContentFrancis"/>
        /// </summary>
        /// <typeparam name="Obj">要序列化的对象的类型</typeparam>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="options">控制序列化过程的选项</param>
        /// <param name="jsonType">如果这个值为<see langword="true"/>，则媒体类型为Json，
        /// 否则为纯文本，它在某些特殊情况下可能会有用</param>
        /// <returns></returns>
        public static HttpContentFrancis HttpContentJson<Obj>(Obj? obj, JsonSerializerOptions? options = null, bool jsonType = true)
        {
            var content = new HttpContentFrancis(JsonContent.Create(obj, options: options), true);
            return jsonType ? content : content with
            {
                Header = content.Header with
                {
                    ContentType = MediaTypeHeaderValue.Parse(MediaTypeName.Text)
                }
            };
        }
        #endregion
        #endregion
    }
}
