using System;
using System.Safety;
using System.Safety.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Microsoft.AspNetCore.Authentication
{
    /// <summary>
    /// 这个类型是<see cref="IHttpAuthentication"/>的实现，
    /// 它通过Http请求的Authentication标头以及Cookie来确认身份，
    /// 但不会将验证结果记住，这个操作由前端负责
    /// </summary>
    class HttpAuthentication : IHttpAuthentication
    {
        #region 说明文档
        /*问：为什么本类型要从标头和Cookie中获取两次凭据？
          答：因为作者注意到，如果请求来自浏览器，在Cookie中携带身份信息比较方便，
          如果请求是通过代码直接发起（例如使用HttpClient），则在Authentication标头中携带身份信息比较方便，
          本接口不应假设请求来自何处，因此需要同时兼顾这两种方式*/
        #endregion
        #region 封装的对象
        #region 通过标头提取用户名和密码的委托
        /// <summary>
        /// 这个委托的参数是HTTP请求的Authentication标头，
        /// 返回值是提取到的用户名和密码，如果提取失败，则返回<see langword="null"/>
        /// </summary>
        private Func<StringValues, UnsafeCredentials?> ExtractionHeader { get; }
        #endregion
        #region 通过Cookie提取用户名和密码的委托
        /// <summary>
        /// 这个委托的参数是附加在请求中的Cookie，
        /// 返回值是提取到的用户名和密码，如果提取失败，则返回<see langword="null"/>
        /// </summary>
        private Func<IRequestCookieCollection, UnsafeCredentials?> ExtractionCookie { get; }
        #endregion
        #region 验证用户名和密码的委托
        /// <summary>
        /// 这个委托的参数是待验证的用户名和密码，
        /// 返回值是验证结果
        /// </summary>
        private Func<UnsafeCredentials, Task<ClaimsPrincipal>> VerifyUser { get; }
        #endregion
        #endregion
        #region 接口实现
        #region 验证Http上下文
        public async Task<ClaimsPrincipal> Verify(HttpContext context)
        {
            var request = context.Request;
            var results = request.Headers.TryGetValue("Authentication", out var authentication) &&
                 ExtractionHeader(authentication) is { } credentialsHeader ?
                 await Verify(credentialsHeader) :
                 ExtractionCookie(request.Cookies) is { } credentialsCookie ?
                 await Verify(credentialsCookie) : CreateSafety.PrincipalDefault;
            if (results.IsAuthenticated())
                context.User = results;
            return results;
        }
        #endregion
        #region 验证用户名和密码
        public Task<ClaimsPrincipal> Verify(UnsafeCredentials credentials)
            => VerifyUser(credentials);
        #endregion
        #region 写入验证
        public Task SetVerify(ClaimsPrincipal credentials, HttpContext context)
            => Task.CompletedTask;
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="VerifyUser">这个委托的参数是待验证的用户名和密码，返回值是验证结果</param>
        /// <param name="ExtractionHeader">这个委托的参数是HTTP请求的Authentication标头，返回值是提取到的用户名和密码，如果提取失败，则返回<see langword="null"/></param>
        /// <param name="ExtractionCookie">这个委托的参数是附加在请求中的Cookie，返回值是提取到的用户名和密码，如果提取失败，则返回<see langword="null"/></param>
        public HttpAuthentication(Func<UnsafeCredentials, Task<ClaimsPrincipal>> VerifyUser,
            Func<StringValues, UnsafeCredentials?> ExtractionHeader,
            Func<IRequestCookieCollection, UnsafeCredentials?> ExtractionCookie)
        {
            this.VerifyUser = VerifyUser;
            this.ExtractionHeader = ExtractionHeader;
            this.ExtractionCookie = ExtractionCookie;
        }
        #endregion
    }
}
