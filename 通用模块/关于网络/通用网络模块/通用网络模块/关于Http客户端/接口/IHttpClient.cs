using System.Threading.Tasks;

namespace System.NetFrancis.Http
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以用来发起Http请求
    /// </summary>
    public interface IHttpClient
    {
        #region 说明文档
        /*问：BCL有原生的HttpClient，
          既然如此，为什么需要本类型？
          答：因为原生的HttpClient是可变的，
          而且经常在多个线程甚至整个应用中共享同一个HttpClient对象，
          这种操作非常危险，而本接口的所有API都是纯函数，消除了这个问题

          同时，本接口使用IHttpRequestRecording来封装提交Http请求的信息，
          根据推荐做法，通常使用该接口的实现HttpRequestRecording，
          它是一个记录，通过C#9.0新支持的with表达式，能够更加方便的替换请求内容的任何部分*/
        #endregion
        #region 发起Http请求
        /// <summary>
        /// 发起Http请求，并返回结果
        /// </summary>
        /// <param name="request">请求消息的内容</param>
        /// <returns></returns>
        Task<IHttpResponse> SendAsync(IHttpRequestRecording request);
        #endregion
    }
}
