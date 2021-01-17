using System.Design.Direct;
using System.NetFrancis;
using System.Safety.Authentication;

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
        /// <param name="obj">这个<see cref="IDirect"/>封装了用户名和密码</param>
        /// <returns></returns>
        [HttpPost]
        [Consumes(MediaTypeName.Json)]
        public bool Login(IDirect obj, [FromServices] IHttpAuthentication authentication)
        {
            //HttpContext.User = authentication.Verify(new UnsafeCredentials(obj["uid"]!.ToString()!, obj["pwd"]!.ToString()!)).Result;
            return true;
        }
        #endregion
    }
}
