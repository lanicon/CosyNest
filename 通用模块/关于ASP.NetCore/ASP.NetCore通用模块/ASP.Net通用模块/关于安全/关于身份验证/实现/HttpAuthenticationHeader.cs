using System;
using System.Safety.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Authentication
{
    /// <summary>
    /// 这个类型是<see cref="IHttpAuthentication"/>的实现，
    /// 它通过Http请求的Authentication标头来确认身份，
    /// 但不会将验证结果记住，这个操作由前端负责
    /// </summary>
    class HttpAuthenticationHeader : IHttpAuthentication
    {
        #region 接口实现
        #region 验证Http上下文
        public Task<ClaimsPrincipal> Verify(HttpContext context)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region 验证用户名和密码
        public Task<ClaimsPrincipal> Verify(UnsafeCredentials credentials)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region 写入验证
        public Task SetVerify(ClaimsPrincipal credentials, HttpContext context)
            => Task.CompletedTask;
        #endregion 
        #endregion
    }
}
