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
    /// 它通过Http请求的Authentication标头来确认身份，
    /// 但不会将验证结果记住，这个操作由前端负责
    /// </summary>
    class HttpAuthenticationHeader : IHttpAuthentication
    {
        #region 封装的对象
        #region 通过标头提取用户名和密码的委托
        /// <summary>
        /// 这个委托的参数是HTTP请求的Authentication标头，
        /// 返回值是提取到的用户名和密码
        /// </summary>
        private Func<StringValues, Task<UnsafeCredentials>> ExtractionUser { get; }
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
            if (context.Request.Headers.TryGetValue("Authentication", out var authentication))
            {
                var credentials = await ExtractionUser(authentication);
                return await Verify(credentials);
            }
            return CreateSafety.PrincipalDefault;
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
        /// <param name="ExtractionUser">这个委托的参数是HTTP请求的Authentication标头，返回值是提取到的用户名和密码</param>
        /// <param name="VerifyUser">这个委托的参数是待验证的用户名和密码，返回值是验证结果</param>
        public HttpAuthenticationHeader(Func<StringValues, Task<UnsafeCredentials>> ExtractionUser, Func<UnsafeCredentials, Task<ClaimsPrincipal>> VerifyUser)
        {
            this.ExtractionUser = ExtractionUser;
            this.VerifyUser = VerifyUser;
        }
        #endregion
    }
}
