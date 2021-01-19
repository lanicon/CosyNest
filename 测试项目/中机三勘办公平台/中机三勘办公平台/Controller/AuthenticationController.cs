using System.Design.Direct;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Components.Controller
{
    /// <summary>
    /// 这个控制器被用来处理登录和注销
    /// </summary>
    [Mvc.Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region 登录
        /// <summary>
        /// 执行登录操作，并返回登录结果
        /// </summary>
        /// <returns></returns>
        public async Task<IDirect> Login([FromServices] IHttpAuthentication authentication)
            => (await ToolWebApi.VerifySerialization(() => authentication.Verify(HttpContext))).Serialization;
        #endregion
    }
}
