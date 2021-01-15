using System.Net.Http;
using System.Threading.Tasks;

namespace System.NetFrancis.Http
{
    /// <summary>
    /// 这个类型是<see cref="IHttpClient"/>的实现，
    /// 可以用来发起Http请求
    /// </summary>
    class HttpClientRealize : IHttpClient
    {
        #region 封装的HttpClient对象
        /// <summary>
        /// 获取封装的<see cref="Http.HttpClient"/>对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private static HttpClient HttpClient { get; } = new();
        #endregion
        #region 发起Http请求
        public async Task<IHttpResponse> SendAsync(IHttpRequestRecording request)
        {
            using var Request = request.ToHttpRequestMessage();
            using var response = await HttpClient.SendAsync(Request);
            return new HttpResponse(response);
        }
        #endregion 
    }
}
