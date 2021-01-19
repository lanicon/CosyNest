using System.Safety.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Authentication
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以进行Http身份验证
    /// </summary>
    public interface IHttpAuthentication
    {
        #region 验证身份
        #region 指定HttpContext
        /// <summary>
        /// 验证一个Http上下文，并返回验证结果
        /// </summary>
        /// <param name="context">需要验证的Http上下文，
        /// 如果验证通过，还会将结果写入<see cref="HttpContext.User"/>属性</param>
        /// <returns></returns>
        Task<ClaimsPrincipal> Verify(HttpContext context);
        #endregion
        #region 指定用户名和密码
        /// <summary>
        /// 验证用户名与密码，并返回验证结果
        /// </summary>
        /// <param name="credentials">封装了用户名和密码的对象</param>
        /// <returns></returns>
        Task<ClaimsPrincipal> Verify(UnsafeCredentials credentials);
        #endregion
        #endregion
        #region 写入验证结果
        /// <summary>
        /// 将验证结果写入Http上下文，使它能够被持久化
        /// </summary>
        /// <param name="credentials">待写入的验证结果</param>
        /// <param name="context">用来写入验证结果的Http上下文</param>
        Task SetVerify(ClaimsPrincipal credentials, HttpContext context);
        #endregion
    }
}
